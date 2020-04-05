# Suddenly Sleepy - Final Project CS276 - Katie Strauss

**Rich Media**

| Location | Type | Action | Response |
|--|--|--|--|
| Navbar | Logo Image | Click | Redirects user to home page |

**Data Driven**

*Moderate Complexity 3-6 databases*

| Database | Purpose |
|--|--|
| Identity Databases | This category is large, and for the most part not manually coded.  The User database has a few custom properties.  For managing users |
| Donations | records donations made |
| SSEvents | records events |
| SSUserSSEvent | bridging table that is required for a many to many relationship in EF core.  This database is not directly manipulated, but is essential for program functionality. |

*Moderate Complexity 9-30 fields in the databases*

| Database | Field |
|--|--|
| Donations | DonationId </br> DonationAmount </br> DonationDate </br> Donor |
| SSEvents | SSEventId </br> MeetingDate </br> Location </br> SSEventName </br> Description |
| SSUserSSEvent | SSUserId </br> SSEventId </br>

*Users should be able to enter data that will be stored in the database*

| Form | Reason|
|--|--|
| Register | Account creation |
| Donation | donate to the cause |
| Event | create an event |
| Register(button) | Register for an event |

*Some of the data entered by users should be visible to other users*

| Data | Who makes it | Who can see it |
| Donation | Anyone | Users may access their own donation record, if they are logged in when they donate. Administrators can see all donation records |
| Upcoming Events | Admin | Users can see the next event that will occur on the main page.  Future development would permit users to look back through their previously attended avents.  Admin can see all event records |

*Users should be able to do some kind of searching of the database*

| What | What search is done |
| Donation History | the program performs the search for a user's specific donation history for them.  The user may choose to reorganize the data based on different heading columns |
| anything admin| administrators have a full panel of access options to pull up specific records and searches etc. |

**Complexity continued**

*Moderate complesity 8-12 web pages*

| |
|--|
| There are 14 distinct views for Administrators alone |
| There are 3 views in the home controller that are available for anyone to see |
| Login and Registration |
| There are 2 views that users access based on their actions |
| total 21 pages |

*The website should have some kind of navigation that appears on each page*

| |
|--|
| The shared _layout view contains a navbar that is shared across all pages|

**Authentication and Authorization**

|Who | What can they do (in addition to preceeding levels) |
|--|--|
| Guest | Guests may donate (anonymously), they may view the home page, they may view the about page, and they may register or access the login page |
| Member | Members may access the pages that Guests can.  In addition, their donations are linked to their userId, they may register to attend avents, and they can see a record of their donations |
| Admin | Admin can do whatever they want to.  Admin can create events, access all records of all things, admin have all the power. |

**Security Testing**

| |
|--|
| No high security risks were found.  Contained within the docs folder is a screenshot of all risks discovered |

**Deploy to a web server**

| |
|--|
| Deployed to Azure |

**Testing**

| |
|--|
| This app was developed interacting directly through the contexts instead of through repositories.  There are unit tests for the basic functionality of the Donation and SSEvent models via fake repositories, but having not covered Moq and having identity integrated into all controllers that would benefit from testing, API testing through postman was used instead.  API classes are contained within the main project, however when not testing them I have restricted access to them to admin only. |