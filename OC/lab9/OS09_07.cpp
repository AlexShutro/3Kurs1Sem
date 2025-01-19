#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <unistd.h>

int main() {
    int file = open("example.txt", O_RDWR | O_CREAT, 0666);
    if (file == -1) {
        perror("Failed to open file");
        return 1;
    }

    // Запись данных в начало файла
    const char* text1 = "Hello, ";
    if (write(file, text1, 7) == -1) {
        perror("Failed to write to file");
        close(file);
        return 1;
    }

    // Сдвиг указателя на 10 байт от начала файла
    if (lseek(file, 10, SEEK_SET) == -1) {
        perror("Failed to lseek");
        close(file);
        return 1;
    }

    // Запись данных после смещения указателя
    const char* text2 = "world!";
    if (write(file, text2, 6) == -1) {
        perror("Failed to write to file");
        close(file);
        return 1;
    }

    // Переход к началу файла и чтение данных
    if (lseek(file, 0, SEEK_SET) == -1) {
        perror("Failed to lseek");
        close(file);
        return 1;
    }

    char buffer[20];
    ssize_t bytesRead = read(file, buffer, sizeof(buffer) - 1);
    if (bytesRead == -1) {
        perror("Failed to read file");
        close(file);
        return 1;
    }
    buffer[bytesRead] = '\0';
    printf("File content: %s\n", buffer);

    close(file);
    return 0;
}
