#include <iostream>
#include <Windows.h>
#include <ctime>
using namespace std;

int main() {
    DWORD pid = GetCurrentProcessId();
    cout << "Main process ID: " << pid << '\n';

    long long it = -10 * 10000000;
    HANDLE htimer = CreateWaitableTimer(NULL, FALSE, L"os07_04");
    if (!SetWaitableTimer(htimer, (LARGE_INTEGER*)&it, 10000, NULL, NULL, FALSE))
        throw "SetWaitableTimer Error";

    LPCWSTR path = L"D:\\3 kurs\\1 sem\\ос\\лабы\\lab7\\OS07_04x\\x64\\Debug\\OS07_04x.exe";

    PROCESS_INFORMATION pi1, pi2;
    clock_t start = clock();
    pi1.dwThreadId = 1; pi2.dwThreadId = 2;
    {
        STARTUPINFO si; ZeroMemory(&si, sizeof(STARTUPINFO)); si.cb = sizeof(STARTUPINFO);
        CreateProcess(path, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &pi1) ?
            cout << "Process os07_04x #1 was created\n" : cout << "Process os07_04x #1 wasn't created\n";
    }
    {
        STARTUPINFO si; ZeroMemory(&si, sizeof(STARTUPINFO)); si.cb = sizeof(STARTUPINFO);
        CreateProcess(path, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &pi2) ?
            cout << "Process os07_04x #2 was created\n" : cout << "Process os07_04x #2 wasn't created\n";
    }

    WaitForSingleObject(pi1.hProcess, INFINITE);
    WaitForSingleObject(pi2.hProcess, INFINITE);

    CloseHandle(htimer);
    cout << "Elapsed time = " << clock() - start << " clocks\n";
    system("pause");
    return 0;
}