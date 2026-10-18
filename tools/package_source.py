"""Create a source-only submission using an explicit allowlist."""
import argparse, hashlib, re, shutil, zipfile
from pathlib import Path

parser = argparse.ArgumentParser()
parser.add_argument('--student', required=True)
args = parser.parse_args()
student = args.student.strip()
if len(student.split()) < 2: parser.error('Öğrenci adı ve soyadı gereklidir.')
name = re.sub(r'[^\w-]+', '_', student, flags=re.UNICODE).strip('_')
root = Path(__file__).resolve().parents[1]
out = root / 'output'
out.mkdir(exist_ok=True)
files = []
excluded = {'bin','obj','App_Data','__pycache__'}
for directory in ['src','tests','docs','screenshots','tools']:
    for path in (root/directory).rglob('*'):
        if not path.is_file() or excluded.intersection(path.relative_to(root).parts): continue
        if path.name == 'appsettings.Local.json' or path.suffix in {'.db','.log','.pyc','.user'}: continue
        files.append(path)
for filename in ['AutomotiveServiceManagement.sln','Directory.Build.props','global.json','dotnet-tools.json','.gitignore','README.md','Start.ps1','Test.ps1']:
    files.append(root/filename)
archive = out / f'{name}_AutomotiveServiceManagement.zip'
with zipfile.ZipFile(archive,'w',zipfile.ZIP_DEFLATED) as z:
    for path in sorted(files): z.write(path,Path('AutomotiveServiceManagement')/path.relative_to(root))
with zipfile.ZipFile(archive) as z:
    assert z.testzip() is None
    assert any(n.endswith('/Data/Migrations/ServiceDbContextModelSnapshot.cs') for n in z.namelist())
    assert not any('/bin/' in n or '/obj/' in n or '/App_Data/' in n for n in z.namelist())
image = out/f'{name}_Screenshot.jpg'
shutil.copyfile(root/'screenshots/dashboard.jpg',image)
for path in [archive,image]:
    digest = hashlib.sha256(path.read_bytes()).hexdigest()
    print(f'{path.name}: {path.stat().st_size:,} bytes | SHA256 {digest}')
