"""Runs against an isolated temporary SQLite database; never changes the demo database."""
import html, http.cookiejar, os, re, subprocess, tempfile, time, urllib.error, urllib.parse, urllib.request
from pathlib import Path
root = Path(__file__).resolve().parents[1]
base = 'http://127.0.0.1:5181'
opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()))
count = 0
def request(path, data=None):
    body = urllib.parse.urlencode(data).encode() if data is not None else None
    try:
        with opener.open(base+path, body, timeout=15) as r: return r.status, r.read().decode(), r.url
    except urllib.error.HTTPError as e: return e.code, e.read().decode(), e.url
def hidden(doc, name):
    for tag in re.findall(r'<input\b[^>]*>', doc):
        attrs = dict(re.findall(r'([\w-]+)="([^"]*)"',tag))
        if attrs.get('name','').lower() == name.lower(): return html.unescape(attrs.get('value',''))
    raise AssertionError('Missing hidden input: '+name)
def post(path, fields, source):
    return request(path, dict(fields, __RequestVerificationToken=hidden(source,'__RequestVerificationToken')))
def check(label, condition):
    global count
    assert condition, label
    count += 1
    print(f'PASS {count:02}: {label}')
with tempfile.TemporaryDirectory(prefix='mcsd-http-') as tmp:
    env = dict(os.environ, ConnectionStrings__ServiceDb=f'Data Source={tmp}/test.db;Foreign Keys=True', SeedDemoData='false', ASPNETCORE_ENVIRONMENT='Production')
    with open(Path(tmp)/'server.log','w',encoding='utf-8') as log:
        server = subprocess.Popen(['dotnet',str(root/'src/Web.UI/bin/Debug/net10.0/Web.UI.dll'),'--urls',base],cwd=root/'src/Web.UI',env=env,stdout=log,stderr=log)
        try:
            for _ in range(100):
                try:
                    if request('/')[0] == 200: break
                except (OSError, urllib.error.URLError): time.sleep(.1)
            else: raise RuntimeError('Server did not start')
            for path in ['/', '/Customers','/Vehicles','/Parts','/Services','/Home/Help','/Customers/Edit','/Vehicles/Edit','/Parts/Edit','/Services/Create']:
                status, doc, _ = request(path); check('GET '+path, status == 200 and 'lang="tr"' in doc)
            check('404 missing record', request('/Vehicles/Details/999999')[0] == 404)
            check('GET cannot delete', request('/Customers/Delete/1')[0] == 405)
            for path in ['/Customers/Edit','/Vehicles/Edit','/Parts/Edit','/Parts/Receive','/Services/Create','/Services/AddPart','/Services/ChangeStatus','/Customers/Delete','/Vehicles/Delete','/Parts/Delete']:
                check('CSRF rejected '+path, request(path,{})[0] == 400)
            doc = request('/Customers/Edit')[1]
            status, invalid, _ = post('/Customers/Edit',{'Name':'','Phone':'x'},doc)
            check('Server validation displays Turkish errors',status == 200 and 'Ad soyad giriniz.' in html.unescape(invalid))
            status, listing, _ = post('/Customers/Edit',{'Name':'HTTP Müşteri','Phone':'05320000000','Email':'http@example.com'},doc)
            check('Create customer and redirect',status == 200 and 'HTTP Müşteri' in html.unescape(listing))
            customer_id = re.search(r'/Customers/Details/(\d+)',listing).group(1)
            doc = request('/Vehicles/Edit')[1]
            _, listing, _ = post('/Vehicles/Edit',{'CustomerId':customer_id,'Plate':'06 HTTP 123','Brand':'Ford','Model':'Focus','Year':2020,'Mileage':1000},doc)
            check('Create vehicle and normalize plate','06HTTP123' in listing)
            vehicle_id = re.search(r'/Vehicles/Details/(\d+)',listing).group(1)
            doc = request('/Parts/Edit')[1]
            status, invalid, _ = post('/Parts/Edit',{'Code':'BAD-01','Name':'Invalid money','Price':'1.250','MinimumStock':3},doc)
            check('Ambiguous thousands/sub-cent input rejected','Binlik ayırıcı' in html.unescape(invalid))
            status, listing, _ = post('/Parts/Edit',{'Code':'HTTP-01','Name':'HTTP parça','Price':'125,50','MinimumStock':3,'Stock':99999},doc)
            check('Turkish comma price accepted',status == 200 and '125,50' in listing)
            part_id = re.search(r'/Parts/Details/(\d+)',listing).group(1)
            doc = request('/Parts/Details/'+part_id)[1]
            check('Overposted stock is ignored','0 adet' in doc)
            check('Zero stock has visible warning', 'Stok tükendi' in html.unescape(request('/Parts')[1]))
            _, detail, _ = post('/Parts/Receive',{'Id':part_id,'Version':hidden(doc,'version'),'Quantity':10},doc)
            check('Stock receipt via protected POST','10 adet' in detail)
            check('Healthy stock has visible status','Yeterli stok' in html.unescape(request('/Parts')[1]))
            doc = request('/Services/Create')[1]
            _, detail, url = post('/Services/Create',{'VehicleId':vehicle_id,'Date':time.strftime('%Y-%m-%d'),'Mileage':1200,'Description':'HTTP bakım işlemi','Labor':'500,25'},doc)
            check('Service created via MVC form','SRV-0001' in detail)
            service_id = url.rsplit('/',1)[1]
            _, invalid_part, _ = post('/Services/AddPart',{'Id':service_id,'Version':hidden(detail,'version'),'PartId':0,'Quantity':2},detail)
            check('Missing part selection gets specific Turkish error and field marker','Parça seçiniz.' in html.unescape(invalid_part) and 'aria-invalid="true"' in invalid_part)
            check('Invalid selection leaves stock unchanged','10 adet' in request('/Parts/Details/'+part_id)[1])
            _, detail, _ = post('/Services/AddPart',{'Id':service_id,'Version':hidden(detail,'version'),'PartId':part_id,'Quantity':2},detail)
            check('Service total includes labor and parts','751,25' in detail)
            check('Stock reduced after part use','8 adet' in request('/Parts/Details/'+part_id)[1])
            edit = request('/Parts/Edit/'+part_id)[1]
            post('/Parts/Edit',{'Id':part_id,'Version':hidden(edit,'Version'),'Code':'HTTP-01','Name':'HTTP parça','Price':'125,50','MinimumStock':8},edit)
            stock_page = html.unescape(request('/Parts')[1])
            check('Stock equal to threshold is visibly critical','Stok uyarısı:' in stock_page and 'Kritik stok' in stock_page and 'table-warning' in stock_page)
            _, detail, _ = post('/Services/ChangeStatus',{'Id':service_id,'Version':hidden(detail,'version'),'Status':'Completed'},detail)
            check('Service completed and locked','Bu dosya kapat' in html.unescape(detail))
            check('Dashboard includes completed service', '751' in request('/')[1])
            doc = request('/Customers/Details/'+customer_id)[1]
            _, detail, _ = post('/Customers/Delete',{'Id':customer_id,'Version':hidden(doc,'version')},doc)
            check('Related customer deletion blocked','bağlı araçlar' in html.unescape(detail))
            print(f'SUCCESS: {count} HTTP checks passed.')
        finally:
            server.terminate()
            server.wait(timeout=15)
