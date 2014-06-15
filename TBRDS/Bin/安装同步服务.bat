@echo off
set/p n=即将开始安装服务是否继续? 输入 y 继续,输入 n 退出.
if /i %n% neq y exit/b

sc stop UltraRDS

%systemroot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u Ultra.RDS.SyncSvc.exe
%systemroot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /i Ultra.RDS.SyncSvc.exe