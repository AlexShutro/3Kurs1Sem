#include <iostream>
#include <Windows.h>
using namespace std;

PROCESS_INFORMATION createProcess(LPCWSTR path)
{
	STARTUPINFO startupInfo;
	PROCESS_INFORMATION processInfo;

	ZeroMemory(&startupInfo, sizeof(STARTUPINFO));
	startupInfo.cb = sizeof(STARTUPINFO);

	if (CreateProcessW(path, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &startupInfo, &processInfo))
		cout << "Process created.\n";
	else
		cout << "[ERROR]\n";

	return processInfo;
}

int main()
{
	int pid = GetCurrentProcessId();
	const int size = 2;
	HANDLE event;
	HANDLE processes[size];

	processes[0] = createProcess(L"D:\\3 kurs\\1 sem\\ос\\лабы\\lab6\\OS06_05A\\x64\\Debug\\OS06_05A.exe").hProcess;
	processes[1] = createProcess(L"D:\\3 kurs\\1 sem\\ос\\лабы\\lab6\\OS06_05B\\x64\\Debug\\OS06_05B.exe").hProcess;

	event = CreateEvent(NULL, FALSE, FALSE, L"OS07_05");

	for (int i = 1; i <= 90; i++)
	{
		if (i == 1)
			SetEvent(event);

		printf("[OS06_05]\t %d.  PID = %d\n", i, pid);
		Sleep(100);
	}

	WaitForMultipleObjects(size, processes, TRUE, INFINITE);
	for (int i = 0; i < size; i++)
		CloseHandle(processes[i]);

	CloseHandle(event);
	system("pause");
	return 0;
}