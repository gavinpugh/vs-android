@echo off
del release.zip
"C:\Program Files\7-Zip\7z.exe" a release.zip license.txt readme.txt MSBuild\*DLL MSBuild\install_* MSBuild\Android\*
