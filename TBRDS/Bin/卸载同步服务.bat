@echo off
set/p n=确定要卸载所有服务吗? 输入 y 继续卸载,输入 n 退出.
if /i %n% neq y exit/b

sc stop UltraRDS
%systemroot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u Ultra.RDS.SyncSvc.exe