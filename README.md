# 🏦 Console Bank System (File I/O & xUnit)

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Testing](https://img.shields.io/badge/xUnit-Testing-success?style=for-the-badge)
![Architecture](https://img.shields.io/badge/Architecture-Clean_Code-blue?style=for-the-badge)

## 📌 Project Overview
A robust, console-based Banking Management System built with C#. This project demonstrates advanced backend logic without relying on a database, utilizing **File I/O streams** for persistent storage. The core focus is on building a fail-safe environment, rigorous state management, and ensuring reliability through automated testing.

## 🏗️ Core Engineering Highlights

* **Clean Folder Architecture:** Strict separation of concerns divided into `Data`, `Models`, `Services`, `UI`, and `Utilities`.
* **Advanced C# Features:** * Implementation of `Delegates` and `Func<T>` (in `UIHelper.cs`) to abstract and encapsulate transaction execution logic, strictly adhering to the DRY principle.
  * Extensive use of **LINQ** for efficient data querying and manipulation.
* **Robust Exception Handling (Fail-Safe):** * Custom `ErrorHandler` intercepting specific exceptions (`InvalidDataException`, `IOException`) to prevent system crashes during file manipulation or corrupted data parsing.
* **Zero Trust Data Validation:** Full validation on domain entities (e.g., `Client`) ensuring no invalid state (like negative balances or corrupted IDs) can exist in memory.
* **Automated Unit Testing:** A dedicated testing project utilizing **xUnit**. Covering edge cases, data validation rules, and business logic using `[Fact]` and `[Theory]/[InlineData]`.

## 📁 Solution Structure
├── Data/ (File handling and storage logic)
├── Models/ (Domain entities and validation)
├── Services/ (Business logic and banking operations)
├── UI/ (Delegates and user interactions)
├── Utilities/ (Error handling and logging)
└── BankManager.Tests/ (xUnit test cases)
