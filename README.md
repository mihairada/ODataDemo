# ODataDemo
ODataDemo
This is a small demo of OData REST Calls with a SQL Server backend including lessons learned.

To set-up: 
1. Restore the App_Data\ODataDemoProducts.bak SQL BD Backup on a SQL instance and point the web.config ProductContext connection string to it.
2. Go to project properties and under Web/Servers choose Local IIS and then Create Virtual Directory. That should create a virtual directory point to the solution

To use:

Either start debugging (F5) in Visual Studio which should pop up a browser and the use OData calls to interact with the Product Controller
or just navigate to http://localhost/ODataDemo/OdataApi/Products in a browser to send GET requests.
For POST or any other type of requests Fiddler / Composer can be used:
Examples:
Create a product: 
POST /ODataAPI/Products HTTP/1.1 
{"Name":"Mihai Test 5", "ProductCategoryId": 2}
