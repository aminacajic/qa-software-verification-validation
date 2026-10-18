# Software Quality Assurance | Verification & Validation

**University Project | Faculty of Electrical Engineering, University of Sarajevo | 2025/2026**

A comprehensive university project focused on Software Quality Assurance (QA), Software Verification and Validation (V&V), covering software testing methodologies, code inspection, test automation, software quality metrics, and performance evaluation.

The project combines practical experience in white-box and black-box testing, including unit testing, test-driven development (TDD), code coverage analysis, UI automation, API testing, and performance testing.

## Project Overview

The project was completed as part of the Software Verification and Validation (VVS) course at the Faculty of Electrical Engineering, University of Sarajevo, Department of Computing and Informatics.

It consists of three main parts:

1. **Code Inspection, Unit Testing & Test-Driven Development**
2. **White-Box Testing, Code Coverage & Software Quality Analysis**
3. **Black-Box Testing, Test Automation, API & Performance Testing**

---

## Part 1: Code Inspection, Unit Testing & TDD

**Application:** ForBooks – C#/.NET book management application

This part focuses on identifying software defects, verifying application functionality, and implementing automated unit tests.

### Activities

- Performed static code inspection to identify potential defects and code quality issues.
- Used inspection checklists and documented identified defects.
- Designed and implemented unit tests using MSTest.
- Created positive, negative, boundary-value, and data-driven test cases.
- Used fake database implementations to isolate components during testing.
- Applied Test-Driven Development (TDD) principles.
- Implemented and tested functionality related to Wish Lists and View History.
- Evaluated test results and code coverage.

### Technologies

`C#` ` .NET` `MSTest` `TDD` `Unit Testing`

**Documentation:**
- [Code Inspection](VVS_1dioprojekta/VVS-Inspekcija%20koda.pdf)
- [Unit Testing](VVS_1dioprojekta/VVS-unit%20testiranje.pdf)
- [Test-Driven Development](VVS_1dioprojekta/VVS-TDD.pdf)

**Source code:** [ForBooks](ForBooks/)

---

## Part 2: White-Box Testing & Software Quality Analysis

**Focus:** Structural testing, code coverage, complexity analysis, refactoring, and performance optimization.

This part focuses on analyzing and improving the `ObrisiNeprimjereneRecenzije` method within a C# application.

### Activities

- Analyzed source code using white-box testing techniques.
- Calculated cyclomatic complexity using McCabe's metric.
- Created control flow graphs and identified independent execution paths.
- Designed test cases for:
  - Statement coverage
  - Branch coverage
  - Condition coverage
  - Modified Condition/Decision Coverage (MC/DC)
  - Loop testing
  - Path coverage
- Analyzed code complexity, maintainability, and coupling.
- Applied refactoring techniques to improve code readability and maintainability.
- Optimized the implementation using techniques such as `HashSet` and `StringComparison.OrdinalIgnoreCase`.
- Compared execution time and memory consumption before and after optimization.

### Technologies & Techniques

`C#` `White-Box Testing` `Code Coverage` `Cyclomatic Complexity` `Refactoring` `Performance Analysis`

**Documentation:** [White-Box Testing & Software Quality Analysis](VVS_2dioprojekta_AmininDio.pdf)

---

## Part 3: Black-Box Testing, Test Automation & Performance Testing

**Applications tested:** TurboSquid and Ogimil

This part focuses on functional software testing, test management, automated UI testing, API validation, and performance testing.

### Manual & Functional Testing

- Designed and executed functional test cases using black-box testing techniques.
- Tested application features including registration, login, search, filtering, and purchasing workflows.
- Documented test cases, execution results, and identified defects.
- Used Azure DevOps for test management and defect tracking.

### UI Test Automation

- Implemented automated UI tests using Selenium WebDriver and NUnit.
- Applied the Page Object Model (POM) design pattern.
- Automated user interactions and functional workflows.
- Verified expected application behavior through automated assertions.

### API Testing

- Created and executed REST API tests using Postman.
- Validated HTTP responses, status codes, and response data.
- Used JavaScript assertions to verify API behavior.

### Performance Testing

- Used Loadster for performance and load testing.
- Evaluated application behavior under simulated user load.

### Technologies

`Selenium WebDriver` `NUnit` `Page Object Model` `Postman` `JavaScript` `Azure DevOps` `Loadster`

**Documentation:** [Black-Box Testing & Test Automation](VVS_3dioprojekta.pdf)

---

## Technologies & Tools

| Category | Technologies / Tools |
|---|---|
| Programming | C#, JavaScript |
| Unit Testing | MSTest, NUnit |
| UI Automation | Selenium WebDriver, Page Object Model |
| API Testing | Postman |
| Test Management | Azure DevOps |
| Performance Testing | Loadster |
| Testing Methodologies | Black-Box, White-Box, TDD |
| Quality Analysis | Code Coverage, Cyclomatic Complexity, Refactoring |
| Version Control | Git, GitHub |

## Repository Structure

```text
qa-software-verification-validation/
│
├── ForBooks/
│   ├── ForBooks/
│   ├── Testovi/
│   ├── DataFiles/
│   └── README.md
│
├── VVS_1dioprojekta/
│   ├── VVS-Inspekcija koda.pdf
│   ├── VVS-unit testiranje.pdf
│   └── VVS-TDD.pdf
│
├── VVS_2dioprojekta_AmininDio.pdf
│
├── VVS_3dioprojekta.pdf
│
└── README.md
```

## Key Learning Outcomes

Through this project, I gained practical experience in:

- Designing and executing manual and automated test cases.
- Identifying software defects through code inspection and functional testing.
- Applying unit testing and TDD methodologies.
- Measuring software quality using code coverage and complexity metrics.
- Creating maintainable UI automation tests.
- Validating REST APIs and application functionality.
- Using test management and defect tracking tools.
- Evaluating software performance and applying code optimizations.

---

**Academic Year:** 2025/2026

**Institution:** University of Sarajevo – Faculty of Electrical Engineering

**Department:** Computing and Informatics

**Course:** Software Verification and Validation (VVS)