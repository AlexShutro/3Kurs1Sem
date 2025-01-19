#include <stdio.h>

int main() {
    FILE* file = fopen("OS09_05.txt", "r");
    if (file == NULL) {
        printf("Ошибка: не удалось открыть файл.\n");
        return 1;
    }

    int line_count = 0;
    char buffer[256]; // Буфер для чтения строк

    while (fgets(buffer, sizeof(buffer), file)) {
        line_count++;
    }

    fclose(file);

    printf("Количество строк в файле: %d\n", line_count);
    return 0;
}
