#! /bin/bash

comp=$(which gcc) 

if [ "$comp" == "" ]; then
    echo "gcc not found! Please install gcc to continue" && exit
fi 

cd "$1" || exit

echo -n "Compiling... "
SUBSTRING=$(echo "$3"| cut -d'"' -f 2)
if test "$SUBSTRING" != "" ; then
  echo -n "Compiler options: $SUBSTRING... "
else
  echo -n "No additional compiler options found... "
fi

gcc -c cmd.c -o cmd.o
gcc -c scomm.c -o scomm.o
gcc -c "$2" -o bench.o
gcc main.c cmd.o scomm.o bench.o -o CBench $SUBSTRING

echo "Done!"
exit 
