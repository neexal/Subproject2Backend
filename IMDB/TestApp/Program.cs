using IMDB.DataServiceLayer;

var service = new DataService();
var query = service.GetUserById(5);
