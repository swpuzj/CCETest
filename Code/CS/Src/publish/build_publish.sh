#!/bin/bash -il
# author: zj 

. /etc/bashrc

bin=$(cd $(dirname $0); pwd)
echo cd $bin
cd $bin

rm -rf app/* 

cd ../

dotnet publish CCETest.csproj -c Release -o publish/app

if [ $? -ne 0 ] ; then
    echo "dotnet build error"
    exit 1
fi

cd publish

docker build -t ccetest:1.0 .

if [ $? -ne 0 ] ; then
    echo "docker build error"
    exit 1
fi

rm -rf app/*


