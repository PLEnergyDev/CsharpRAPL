#! /bin/bash

comp=$(which gcc) 

if [ "$comp" == "" ]; then
    echo "gcc not found! Please install gcc to continue" && exit 1
fi 

cd "$1" || exit 1

echo -n "Compiling... "
SUBSTRING=$(echo "$3"| cut -d'"' -f 2) || exit 1
if test "$SUBSTRING" != "" ; then 
  echo -n "Compiler options: $SUBSTRING... " || exit 1
else
  echo -n "No additional compiler options found... " || exit 1
fi

#gcc -c cmd.c -o cmd.o || exit 1
#gcc -c scomm.c -o scomm.o || exit 1
#gcc -c "$2" -o bench.o || exit 1
gcc main.c "$2" -o CBench $SUBSTRING  || exit 1

echo "Done!"
exit 0
