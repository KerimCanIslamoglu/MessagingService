# Messaging Service
**Kullanılan teknolojiler:** .Net Core 3.1, Entity Framework Code First, Linq, Asp.Net Identity, MsSQL database, Swagger, NUnit, Docker Compose 



## Authenticate
**api/Authenticate/Register**
**POST**
username ve password adında 2 tane parametre ile çalışır. password parametresi en az 1 büyük harf ve özel karakter istemektedir. Bu method sonucunda kullanıcı oluşturulur.
**Örnek olarak:**
{
```
  "username": "User1",
  "password": "User@1"
```
}

**api/Authenticate/Login**
**POST**
username ve password adında 2 tane parametre ile çalışır. Kullanıcı adı ve şifre doğru ise Bearer Token döndürür.
**Örnek olarak:**
{
```
  "username": "User1",
  "password": "User@1"
```
}


## Message
**api/Message/GetOldMessages**
**GET**
userToName adında string bir parametre ile çalışır. Sonuç olarak authenticate olmuş kullanıcı ile userToName parametresinde verilen kullanıcı ile olan mesajlar döner.
**Örnek olarak:**
{
```
  "userToName": "User1",
```
}

**api/Message/SendMessage**
**POST**
message ve userTo adında 2 adet parametre ile çalışır. Authenticate olmuş kullanıcıdan, userTo parametresinde belirtilen kullanıcıya mesaj atılmasını sağlar.
**Örnek olarak:**
```
{
  "message": "Hello World",
  "userTo": "User2"
}
```
## UserManagement
**api/UserManagement/BlockUser**
**POST**
blockedUserName adında string bir parametre ile çalışır. Authenticate olmuş kullanıcı, blockedUserName parametresi ile gelen kullanıcıyı bloklamış olur. Bunun sonucunda blockedUserName parametresi ile gelen kullanıcı, authenticate olmuş kullanıcıya mesaj atamaz.
**Örnek olarak:**
```
{
  "blockedUserName": "User2"
}
```

