# Software Quality Assurance | Verification & Validation

**University Project | Faculty of Electrical Engineering, University of Sarajevo | 2025/2026**

A university project focused on **Software Quality Assurance (QA)** and **Software Verification and Validation (V&V)**, combining code inspection, unit testing, test-driven development, white-box and black-box testing, test automation, API testing, and software quality evaluation.

## Project Overview

This repository contains work completed for the **Software Verification and Validation (VVS)** course at the Faculty of Electrical Engineering, University of Sarajevo (Computing and Informatics).

The project is organized into three parts:

1. **Code Inspection, Unit Testing & Test-Driven Development**
2. **White-Box Testing & Software Quality Analysis**
3. **Black-Box Testing, Automation, API & Performance Testing**

## Part 1: Code Inspection, Unit Testing & TDD

**Application:** ForBooks — a C#/.NET book management application.

- Inspected source code to identify potential defects and quality issues.
- Documented inspection findings.
- Designed and implemented unit and data-driven tests with **MSTest**.
- Used fake database implementations to isolate tested components.
- Applied **Test-Driven Development (TDD)** to selected application functionality.

**Source code:** [ForBooks](ForBooks/)

**Documentation:**
- [Code Inspection](VVS_1dioprojekta/VVS-Inspekcija%20koda.pdf)
- [Unit Testing](VVS_1dioprojekta/VVS-unit%20testiranje.pdf)
- [Test-Driven Development](VVS_1dioprojekta/VVS-TDD.pdf)

## Part 2: White-Box Testing & Software Quality Analysis

**Focus:** Structural testing, coverage criteria, code metrics, refactoring, and performance comparison.

- Analyzed the `ObrisiNeprimjereneRecenzije` method using white-box testing techniques.
- Evaluated **cyclomatic complexity** and control-flow paths.
- Designed test cases for statement, branch, condition, MC/DC, loop, and path coverage.
- Reviewed maintainability and coupling metrics.
- Applied refactoring and compared execution time and memory usage.

**Documentation:** [White-Box Testing & Software Quality Analysis](VVS_2dioprojekta_AmininDio.pdf)

## Part 3: Black-Box Testing, Test Automation & Performance Testing

**Focus:** Functional testing, UI automation, API validation, test management, and load testing.

- Designed and executed black-box functional test cases.
- Documented defects and managed testing activities with **Azure DevOps**.
- Used **Selenium WebDriver** and the **Page Object Model (POM)** for UI automation.
- Used **Postman** for API testing.
- Used **Loadster** for load and performance testing.

**Documentation:** [Black-Box Testing & Test Automation](VVS_3dioprojekta.pdf)

## Technologies & Tools

| Area | Tools and techniques |
| --- | --- |
| Programming | C#, JavaScript |
| Unit testing | MSTest, NUnit |
| UI automation | Selenium WebDriver, Page Object Model |
| API testing | Postman |
| Test management | Azure DevOps |
| Performance testing | Loadster |
| QA methodologies | Code inspection, TDD, black-box and white-box testing |
| Quality analysis | Code coverage, cyclomatic complexity, refactoring |
| Version control | Git, GitHub |

## Repository Structure

```text
qa-software-verification-validation/
├── ForBooks/
│   ├── ForBooks/
│   ├── Testovi/
│   ├── DataFiles/
│   └── README.md
├── VVS_1dioprojekta/
│   ├── VVS-Inspekcija koda.pdf
│   ├── VVS-unit testiranje.pdf
│   └── VVS-TDD.pdf
├── VVS_2dioprojekta_AmininDio.pdf
├── VVS_3dioprojekta.pdf
└── README.md
```

## Skills Demonstrated

- Manual and automated test case design
- Static inspection and defect identification
- Unit testing and test-driven development
- Code coverage and complexity analysis
- UI and API test execution
- Test management and defect tracking
- Performance testing and code optimization

---

**Institution:** University of Sarajevo — Faculty of Electrical Engineering  
**Department:** Computing and Informatics  
**Course:** Software Verification and Validation (VVS)  
**Academic year:** 2025/2026
