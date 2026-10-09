The problem, target users and why it matters (your research)
Our Team have decided to create an appointment booking application. This App is for patients/customers and staff can view and manage appointments. Our problem comes from trying to track appointments and having one easy place where they can be managed. appointments, especially ones that are made in advanced, are hard to manage and are often forgotten and missed. Having to phone or email to try change or book appointments can be time consuming and frustrating. On our app users are able to book appointments online and view all upcoming appointments, making the experience more convenient for both patients and staff. The staff can view all appointments and can manage appointments(create ,update and delete) from a easy to manage and clearly laid out  dashboard. Our goal is the fast track the booking process and make the experience better for both patients and staff.

An architecture diagram or clear description of how the services fit together
<img width="872" height="431" alt="FullStackApp drawio" src="https://github.com/user-attachments/assets/ee5a3f07-1d49-4084-b723-21517fdf8a1a" />

The live URL of your deployed application
https://appointment-web-h92o.onrender.com
Setup instructions to run the project locally with docker compose up --build
this builds and runs the docker: 
docker compose up --build -d

this runs the docker assuming it's been built already:
docker compose up -d

this stops the docker and all containers:
docker compose down
A list of the environment variables required (referring to .env.example)
PostGres Variables 
host=aws-0-eu-west-2.pooler.supabase.com
port=5432
database=postgres
user=postgres.ezwelwedbpodwodvwddd

Local Deployment
API
api: https://localhost:7083


WEB APP
app: https://localhost:7084

A list of group members and their main contributions
Abigail Bailey - ST10478556
Database development and integration, readme file
Dimpho Motsie - ST10458752
Front end development
Mohamed Shaik - ST10484344
back end development, docker and group admin
Simona Tsigab - ST10490572
docker
