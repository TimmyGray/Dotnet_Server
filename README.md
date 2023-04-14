# Aspnet_server  
First, a few words about full project. It provides construction of various cables and their purchase. It consists of two large parts:

1) Part for seller - https://github.com/TimmyGray/Lovely_Wires; https://github.com/TimmyGray/Lovely_wires_server;  
2) Part for customer - https://github.com/TimmyGray/Buying_Client; https://github.com/TimmyGray/Dotnet_Server; https://github.com/TimmyGray/BuyingLibrary;  
Each part contains front and backend with joint mongo database. Most of features implemented, but not all. If something does't work correctly or doesn't work at all -  
please, write me!In additional,i is writing(Not yet finished) this pet-project for show to potential employer my hard skills. So, it is not real app you should use in your business,ofc=)  

This is a server for the Staff Buying Client application that you could find by this link https://github.com/TimmyGray/Buying_Client;  
All models and services using in the app,are placing in the single library https://github.com/TimmyGray/BuyingLibrary;  
Each controller implemented only methods that are using in frontend (not of all crud operations)  
Simple email sending also implemented  

How to run:  
1) Clone this repo  
2) Clone library repo by this link: https://github.com/TimmyGray/BuyingLibrary and build this project  
3) Add  refference of buying lib to this progect  
4) In the appsettings.json, in the ConnectionStrings section you must set DefaultConnection subsection for connect to mongodb, set DataBase subsection for db name, set AppUrl subsection for set application url and posr listen  
5) In the mail_sender directory make emailsettings.json with thats sections "email" - seller's email , "password" - seller's email password, "name" - seller's name, "host" - server url, "hostport" - server host  
6) Build this project  
7) Run Aspnet_server.exe. You will see "Hello there" when you open browser on AppUrl adress  

  


