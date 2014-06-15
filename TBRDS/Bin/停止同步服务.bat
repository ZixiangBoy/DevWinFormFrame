@echo off
@if "%1"=="y" goto op
set/p n=确定要停止所有服务吗? 输入 y 停止所有服务,输入 n 退出.
if /i %n% neq y exit/b

:op
sc stop UltraRDS
