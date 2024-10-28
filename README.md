Task Management System

A task management web application that supports user role-based task creation, real-time collaboration, and task management features.

Project Description

This application is a task management system that allows users to create, update, and delete tasks. It includes role-based access, where:
•	Admin users can view and manage all tasks.
•	Regular users can view only the tasks they created.
The application also includes basic real-time notifications for task updates using SignalR.

Features

•	User registration and login system
•	Role-based access control
•	Task creation, filtering, and management
•	Real-time notifications on task updates (using SignalR)
•	Filtering tasks by status, task name, and assignee

Technologies Used

•	ASP.NET MVC 5
•	Entity Framework
•	SignalR for real-time notifications
•	SQL Server for database management

Setup Instructions

Follow these steps to run the application locally.

1. Clone the Repository
   - git clone [https://github.com/AnandRajput09/Task_Management_App.git](https://github.com/AnandRajput09/Task_Management_App.git)
   - cd Task_Management_App

2. Open the Solution in Visual Studio
•	Open Visual Studio and select "File" > "Open" > "Project/Solution".
•	Navigate to the cloned repository folder and open the .sln file.

3. Setup the Database
•	Configure your database connection string in the web.config file under <connectionStrings>. Here’s an example:

     <connectionStrings>
         <add name="TaskAppEntities" connectionString="Data Source=YOUR_SERVER;Initial Catalog=TaskManagementDb;Integrated Security=True;" providerName="System.Data.SqlClient" />
     </connectionStrings>

4. Restore Dependencies
•	Ensure the following packages are restored in the project. Visual Studio should automatically restore them, but if not, you can install them manually via NuGet Package Manager:
  - Microsoft.AspNet.SignalR (for real-time notifications)
  - EntityFramework (for database ORM functionality)
  - Microsoft.Owin and Microsoft.Owin.Security (for authentication support)
•	To install manually, go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution, and search for and install these packages as needed.
5. Build the Project
•	In Visual Studio, build the project by selecting  Build  >  Build Solution.

6. Run the Application
•	Start the application by clicking the "Run" button or pressing F5.

7. Access the Application
•	Open your browser and go to  ‘http://localhost:yourport/’  to access the application.

Usage

•	Login as either an Admin or a regular user to access the task management system.
•	Admins can manage all tasks, while regular users can only see tasks they created.

Live Application Link

You can access the live version of this application here: https://anandwebsite.bsite.net/Account/Login

GitHub Repository Link

https://github.com/AnandRajput09/Task_Management_App


![Login_Page](https://github.com/user-attachments/assets/c3a98921-665f-437d-ade5-4e69c7c05d51)
![Home](https://github.com/user-attachments/assets/cb13da72-37cd-4d30-8885-6356917377a6)
![Registration](https://github.com/user-attachments/assets/e88b0b19-5655-482d-8837-889f4368c496)
![SearchBY_Filter](https://github.com/user-attachments/assets/0221f3a9-6cd9-4f50-9657-0c6945765246)
![Profile_Section](https://github.com/user-attachments/assets/9b22622a-f309-4235-ac19-e5bfd660072a)
![Update_Profile](https://github.com/user-attachments/assets/d3c98533-ea33-4417-a0f5-8167524869dd)
![Add_Task](https://github.com/user-attachments/assets/f27d2ff7-be26-4880-9356-cc852473f36c)
![Task_Details](https://github.com/user-attachments/assets/b0a17948-90a3-4724-95a3-6cbc5fec20f0)
![Edit_Task](https://github.com/user-attachments/assets/2cf03dc5-c3cf-456b-8456-f4ad18a11565)
![Delete_Task](https://github.com/user-attachments/assets/2f6a5d32-dcdf-4506-a4be-3f535c86736f)
