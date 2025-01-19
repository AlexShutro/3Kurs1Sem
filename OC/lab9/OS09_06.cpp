#include <stdio.h>
#include <stdlib.h>

int main(int argc, char* argv[]) {
    if (argc != 2) {
        printf("Error need 1 argument \n");
        return 1;
    }

    int param = atoi(argv[1]);
    FILE* input_file = fopen("OS09_05.txt", "r");
    if (input_file == NULL) {
        printf("Can't open OS09_05.TXT\n");
        return 1;
    }

    FILE* output_file;
    if (param % 2 == 0) {
        output_file = fopen("OS09_06_2.txt", "w");
    }
    else {
        output_file = fopen("OS09_06_1.txt", "w");
    }

    if (output_file == NULL) {
        printf("Can't create file\n");
        fclose(input_file);
        return 1;
    }

    char buffer[256];
    int line_number = 1;

    while (fgets(buffer, sizeof(buffer), input_file)) {
        if ((param % 2 == 0 && line_number % 2 == 0) ||
            (param % 2 != 0 && line_number % 2 != 0)) {
            fputs(buffer, output_file);
        }
        line_number++;
    }

    fclose(input_file);
    fclose(output_file);

    printf("File is created\n");
    return 0;
}
