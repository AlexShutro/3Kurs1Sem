#include <iostream>
#include <Windows.h>
using namespace std;

extern "C" void EnterCriticalSectionAssem();
extern "C" void LeaveCriticalSectionAssem();

HANDLE createThread(LPTHREAD_START_ROUTINE func, char* thread_name) {
    DWORD thread_id = NULL;
    HANDLE thread = CreateThread(NULL, 0, func, thread_name, 0, &thread_id);
    if (thread == NULL)
        throw "[ERROR] CreateThread";
    return thread;
}

DWORD WINAPI loop(LPVOID param) {
    char* displayed_name = (char*)param;
    int pid = GetCurrentProcessId();
    int tid = GetCurrentThreadId();

    for (int i = 1; i <= 90; ++i) {
        if (i == 30)
            EnterCriticalSectionAssem();

        printf("%d.\tPID = %d\tTID = %u\tthread: %s\n", i, pid, tid, displayed_name);

        if (i == 60)
            LeaveCriticalSectionAssem();

        Sleep(100);
    }

    cout << "\n==========================  " << displayed_name << " finished" << "  ==========================\n\n";
    return 0;
}

int main() {
    const int size = 2;
    HANDLE threads[size];

    threads[0] = createThread(loop, (char*)"A");
    threads[1] = createThread(loop, (char*)"B");

    WaitForMultipleObjects(size, threads, TRUE, INFINITE);
    for (int i = 0; i < size; i++)
        CloseHandle(threads[i]);

    return 0;
}