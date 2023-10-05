@echo off          
set OUT=/out:a.exe
set TRGT=/target:exe
set RES=
set DBG=
set LNGV=4


if exist local.cmd  call local.cmd 
echo on
rem 
csc /nologo  /platform:x86   %TRGT% %OUT% /unsafe %R% *.cs  %RES% %DBG% /langversion:%LNGV%
rem csc /nologo  /platform:x64   %TRGT% %OUT% /unsafe %R% *.cs  %RES% %DBG% /langversion:4
@echo off


rem /langversion:ISO-2, 3, 4, 5, or Default - наверно это

rem 5	The compiler accepts only syntax that is included in C# 5.0 or lower.
rem 4	The compiler accepts only syntax that is included in C# 4.0 or lower.
rem 3  compiler accepts only syntax that is included in C# 3.0 or lower.
rem ISO-2 (or 2)	The compiler accepts only syntax that is included in ISO/IEC 23270:2006 C# (2.0).