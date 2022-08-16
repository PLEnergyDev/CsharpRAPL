#! /bin/bash

comp=$(which gcc) 

if [ "$comp" == "" ];
then
    echo "gcc not found! Please install gcc to continue" && exit
fi 

cd "$1" || exit

echo "Compiling..."

gcc -c cmd.c -o cmd.o
gcc -c scomm.c -o scomm.o
gcc -c "$2" -o bench.o
gcc main.c cmd.o scomm.o bench.o -o CBench

echo "Done!"
exit 
