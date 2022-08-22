#!/bin/bash

comp=$(which javac) 

if [ "$comp" == "" ]; then
    echo "javac not found! Please install javac to continue" && exit 1
fi 

cd "$1" || exit 1

echo -n "Compiling... "
SUBSTRING=$(echo "$2"| cut -d'"' -f 2) || exit 1
if test "$SUBSTRING" != "" ; then
  echo -n "Compiler options: $SUBSTRING... "
  javac *.java "$SUBSTRING" || exit 1
else
  echo -n "No additional compiler options found... "
  javac *.java || exit 1
fi

jar -cmf MANIFEST.MF JavaBench.jar *.class || exit 1
printf "#!/bin/bash\njava -jar \"%s/JavaBench.jar\" \$1" "$1" > JavaBench.sh || exit 1
sudo chmod +x JavaBench.sh || exit 1 
echo "Done!"
exit 0
