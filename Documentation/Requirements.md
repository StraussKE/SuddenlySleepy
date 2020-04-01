# Suddenly Sleepy - Term Project for Asp.Net - Katie Strauss

**Rich Media**

| Type | Implementation |
|------|----|
|Clickable image| Clicking on the Suddenly Sleepy Banner image will take you to the home page|

**Data Driven**

| Action | Result|
|--|--|
| User registers account | Saved to Identity User database |
| Administrator creates a role | Saved to Role database |
| Administrator assigns a user a role | Saved to Role Manager |
| Guest, RegisteredUser, or Administrator makes a donation | Saved to Donations database|
| User clicks "Hello User" | User can see their donation history |
| Administrator navigates to donation index | Administrator can see all donation histor |
| *events* | *needs implemented* |

**Complexity Part 1**<br>*3 to 6 tables*<br>*9 to 30 properties*

| Table | Properties | Purpose |
|--|--|--|
| Donations | DonationId <br> DonationAmount <br> DonationDate <br> DonorId | Primary Key <br> Value of Donation <br> Date of Donation <br> Foreign Key|
| SSEvents | SSEventId <br> MeetingDate <br> Location <br> SSEventName <br> Description <br> | Primary Key <br> Date of Event <br> Location of Event <br> Name of Event <br> Event Description <br>|
| SSUsers | Identity Properties <br> First Name <br> Last Name <br> | Identity <br> First Name <br> Last Name <br> |
| SSUserSSEvent <br> *bridging table*| SSUserID <br> SSEventId <br> | PK, FK <br> PK, FK |

*Note*<br>*Manual creation of bridging table is essential for many to many relationships in EF Core*<br>*EF 6 supported automatic generation of bridging tables*

**Complexity Part 2/Authentication and Authorization**<br>*8 to 12 web pages*

| Controller | Page or Action | Authorization |
|--|--|--|
| Identity | Register | [AllowAnonymous] |
| Identity | Login | [AllowAnonymous] |
| Identity | LogOut | [AllowAnonymous] |
| Admin | AdminCreateRole | [Authorize(Roles = "_Admin")] |
| Admin | AdminEditRole | [Authorize(Roles = "_Admin")] |
| Admin | AdminEditUser | [Authorize(Roles = "_Admin")] |
| Admin | AdminRoleManagement | [Authorize(Roles = "_Admin")] |
| Admin | AdminUserManagement | [Authorize(Roles = "_Admin")] |
| Donations | Create | [AllowAnonymous] |
| Donations | Delete | [Authorize(Roles = "_Admin")] |
| Donations | Details | [Authorize] |
| Donations | Index | [Authorize(Roles = "_Admin")] |
| Donations | MemberDonatons | [Authorize] |
| Home | About | no restrictions |
| Home | Index | no restrictions |
| SSEvents | *finish making this*| |

