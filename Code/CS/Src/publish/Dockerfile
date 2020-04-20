FROM aspnetcore:3.1-alpine
WORKDIR /app
EXPOSE 80

# 解决docker中的容器无法使用中文的问题
ENV LANG C.UTF-8
WORKDIR /app
COPY app/ /app/

#RUN dpkg -i /app/dmidecode/dmidecode_3.0-4_amd64.deb

ENTRYPOINT ["dotnet", "CCETest.dll"]
