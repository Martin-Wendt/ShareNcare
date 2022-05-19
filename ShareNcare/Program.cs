// See https://aka.ms/new-console-template for more information


using Api;
using Api.Entities;
using Api.Interfaces;


Console.WriteLine("Hello, World!");
var ShareNcare = new ShareNcareAPI();

var user1 = ShareNcare.CreateUser("Hanne");
var user2 = ShareNcare.CreateUser("Wolfgang");
var user3 = ShareNcare.CreateUser("Bent");

List<User> users = new();
users.Add(user1);
users.Add(user2);
users.Add(user3);

var grp = ShareNcare.CreateNewGroup("Tivoli trip", "some description", users);


var pm1 = new Payment(600, "Candy");
var pm2 = new Payment(330, "Entrance");
var pm3 = new Payment(60, "Dinner");

ShareNcare.AddPaymentToGroup(grp, pm1, user1);
ShareNcare.AddPaymentToGroup(grp, pm2, user2);
ShareNcare.AddPaymentToGroup(grp, pm3, user3);

Console.WriteLine("group payments");
ShareNcare.GetGroupPayments(grp);
Console.WriteLine();
Console.WriteLine("user payments");
ShareNcare.GetGroupPaymentsByUser(grp, user2);
Console.WriteLine();
Console.WriteLine("outstanding share for user");
var sharesUser2 = ShareNcare.GetOutstandingSharesForUserInGroup(grp, user2);
Console.WriteLine();
Console.WriteLine("Pay share");
ShareNcare.PayGroupShare(grp, sharesUser2[0]);
Console.WriteLine();
Console.WriteLine("payment outstanding user to user");
ShareNcare.GetUserToUserOutstandingShares(grp, user3, user2);
Console.WriteLine();

Console.WriteLine("Resolve group");
ShareNcare.ResolveGroup(grp);
Console.WriteLine();

Console.WriteLine("Trying to close group");
ShareNcare.CloseGroup(grp);
Console.WriteLine();

// user betal difference
Console.WriteLine(user3.Name +" paying all outstanding shares");
ShareNcare.PayAllOutstandingSharesForUser(grp,user3);
Console.WriteLine();

Console.WriteLine(user2.Name + " paying all outstanding shares");
ShareNcare.PayAllOutstandingSharesForUser(grp, user2);
Console.WriteLine();

Console.WriteLine(user1.Name + " paying all outstanding shares");
ShareNcare.PayAllOutstandingSharesForUser(grp, user1);
Console.WriteLine();

Console.WriteLine("Trying to close group");
ShareNcare.CloseGroup(grp);



Console.WriteLine("eda");
Console.ReadKey();

