# Appointment Booking System

CLDV6212

ICE Task

---

## Overview
A medical practice is facing an issue when patients book appointments. This is due to patients having to phone in, or email, to book, and then a staff member manually records this using paper record. This leads to double bookings and errors.

To resolve this, our team has decided to create an appointment booking system. This will allow patients to book their own appointments for specific time slots, and allow staff to create the timelsots. 

This makes things more convenient for both patients and staff. Staff can view and manage all appointments and timeslots from an easy-to-use and clearly-laid-out staff dashboard, while patients can easily book and view all of their existing appointments from the patient dashboard.

The system features a Web-API and a Web-App that consumes the API.

The API provides a connection to Supabase, which is a PostgreSQL database platform.

---

## System Structure
<img width="872" height="431" alt="FullStackApp drawio" src="https://github.com/user-attachments/assets/ee5a3f07-1d49-4084-b723-21517fdf8a1a" />

---

## Links

Live Application: `https://appointment-web-h92o.onrender.com`

Local Deployment:

From Visual Studio:
- Web-API: `https://localhost:7083`
- Web-App: `https://localhost:7084`

From Docker:
- Web-API: `https://localhost:7083`
- Web-App: `https://localhost:8080`

---

## API Testing

Run the Web-API from Visual Studio.

Go to the following URL: `https://localhost:7083/swagger`

---

## Docker

Setup instructions to run the project locally with Docker.

Run the following commands in PowerShell in the root project folder.

Build and run:

`docker compose up --build -d`

Run:

`docker compose up -d`

Stop:

`docker compose stop`

Stop and delete:

`docker compose down`

## Group Member Contributions

Abigail Bailey (ST10478556):
- Database development and integration
- README.md file
- GitHub workflows

Dimpho Motsie (ST10458752):
- Development of the Web-App

Mohamed Shaik (ST10484344) - Group Leader:
- Development of the Web-API
- API documentation
- Docker
- Live Deployment
- README.mf file

Simona Tsigab (ST10490572):
- Docker
