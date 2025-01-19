#include <iostream>
#include <Windows.h>
using namespace std;

int main()
{
	cout << "-------- OS03_02 --------\n";

	LPCWSTR OS03_02_1 = L"D:\\3 kurs\\1 sem\\ос\\лабы\\OS03_02_1\\x64\\Debug\\OS03_02_1.exe";
	LPCWSTR OS03_02_2 = L"D:\\3 kurs\\1 sem\\ос\\лабы\\OS03_02_2\\x64\\Debug\\OS03_02_2.exe";
	STARTUPINFO si1;								
	STARTUPINFO si2;
	PROCESS_INFORMATION pi1;						
	PROCESS_INFORMATION pi2;
	ZeroMemory(&si1, sizeof(STARTUPINFO));			
	ZeroMemory(&si2, sizeof(STARTUPINFO));
	si1.cb = sizeof(STARTUPINFO);				
	si1.dwFlags = STARTF_USEFILLATTRIBUTE;			
	si1.dwFillAttribute = (DWORD)FOREGROUND_GREEN;	
	si1.lpReserved = NULL;							
	si2.cb = sizeof(STARTUPINFO);
	si2.dwFlags = STARTF_USEFILLATTRIBUTE;
	si2.dwFillAttribute = (DWORD)FOREGROUND_RED;
	si1.lpReserved = NULL;


	if (CreateProcess(
		OS03_02_1,			
		NULL,				
		NULL,				
		NULL,				
		FALSE,				
		CREATE_NEW_CONSOLE,
		NULL,				
		NULL,				
		&si1,				
		&pi1))				
		cout << "[OK] Process OS03_02_1 was created.\n";
	else cout << "[ERROR] Process OS03_02_1 was not created.\n";


	if (CreateProcess(OS03_02_2, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si2, &pi2))
		cout << "[OK] Process OS03_02_2 was created.\n\n";
	else cout << "[ERROR] Process OS03_02_2 was not created.\n\n";


	for (short i = 1; i <= 100; ++i)
	{
		cout << i << ". PID = " << GetCurrentProcessId() << "\n";
		Sleep(1000);
	}


	WaitForSingleObject(pi1.hProcess, INFINITE);	
	WaitForSingleObject(pi2.hProcess, INFINITE);	
	CloseHandle(pi1.hThread);	
	CloseHandle(pi2.hThread);
	CloseHandle(pi1.hProcess);
	CloseHandle(pi2.hProcess);
	return 0;
}