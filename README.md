![readmelogo](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/images/MunchBunchLogo-1.png)

## Description
MunchBunch is an app for foodies! Users can follow each other and share their "memoirs" to view the great places they've been and tasty things they've tried.

Using the Zomato API, users can search restaurants in their primary location and then create, edit or delete memoirs. They can also add restaurants to their wishlist and remove them once they've visited the wishlisted restaurant.

## Live Demo
Check it out here: [MunchBunch](https://aprilrwatson.com)

## Technologies Used
- Zomato API
- ASP.Net
- C#
- Bootstrap
- HTML
- JavaScript
- CSS
- Adobe Illustrator CC (for logo design)

## Screenshots

Login Screen
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB1.png)

____

Search Results for Restaurants
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB2.png)

____

When a user clicks "Ate It," (s)he is redirected to an edit view in which details can be entered.
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB3.png)

____

Memoirs list view
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB4.png)

____

Wishlist list view
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB5.png)

____

Edit view in which user can edit any details of the restaurant visit
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB6.png)

____

Memoir details view
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB7.png)

____

Delete confirmation
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB8.png)

____

User profile details view
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB9.png)

____

List view of fellow Munchers the user follows
![Webpage](https://raw.githubusercontent.com/aprilrochelle/munchBunch/master/MunchBunch/wwwroot/screenshots/MB10.png)


## How to Run
1. Clone down this repo and cd into project.
2. In your terminal/git bash, ```Start MunchBunch.sln```
2. Get a [Zomato API key](https://developers.zomato.com/api)
3. In the project, rename appsettings.json.example file to appsettings.json
4. In appsettings.json file, copy and paste in your API key and change connection string to match your database name.
5. In your package manager console, ```add-migration InitialMigration```
6. Then ```update-database```
8. Run the project

## Contributors
[April Watson](https://github.com/aprilrochelle)

with special thanks to NSS server-side instructors:

[Steve Brownlee](https://github.com/SteveBrownlee) (Lead Instructor)

[Jisie David](https://github.com/jisie) (Capstone Mentor)

[Emily Lemmon](https://github.com/Rian501)

[Jordan Castelloe](https://github.com/jordan-castelloe)