.386
.model flat, C
.data
    check DWORD 0
.code

public EnterCriticalSectionAssem, LeaveCriticalSectionAssem

EnterCriticalSectionAssem PROC
    CriticalSection:
        lock bts check, 0
        jc CriticalSection
    ret
EnterCriticalSectionAssem ENDP

LeaveCriticalSectionAssem PROC
    lock btr check, 0
    ret
LeaveCriticalSectionAssem ENDP

END