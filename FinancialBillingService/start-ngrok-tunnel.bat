@echo off
title VNPAY Tunnel - FinancialBillingService
color 0A

echo ============================================
echo   VNPAY Sandbox Tunnel - localhost:5207
echo ============================================
echo.
echo [1/2] Khoi dong FinancialBillingService...
start "FinancialBillingService" /min cmd /c "cd /d %~dp0FinancialBillingService && dotnet run --no-build"

echo Cho server khoi dong (5 giay)...
timeout /t 5 /nobreak >nul

echo.
echo [2/2] Bat dau Ngrok tunnel tren cong 5207...
echo.
echo Sau khi ngrok hien 'Forwarding https://xxxx.ngrok-free.app'
echo  - Copy URL do
echo  - Cap nhat ReturnUrl trong appsettings.json
echo  - Dang ky TmnCode + HashSecret tai: https://sandbox.vnpayment.vn/devreg/
echo.
echo ============================================
echo   Nhan Ctrl+C hoac dong cua so nay de dung
echo ============================================
echo.

:: Chay ngrok - khi dong cua so nay, ngrok tu dong dung
ngrok http 5207

:: Khi ngrok bi tat, dong also FinancialBillingService
echo.
echo [..] Tat FinancialBillingService...
taskkill /f /fi "WINDOWTITLE eq FinancialBillingService*" >nul 2>&1
taskkill /f /im FinancialBillingService.exe >nul 2>&1

echo.
echo Da dong tat ca. Bam phim bat ky de thoat.
pause >nul
