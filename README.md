# Enova365 Calculator Plugin

This repository contains a **custom plugin** for the **Enova365 ERP**

## Features

### 1. Basic Arithmetic Operations
The plugin provides a simple **calculator** supporting:
- Addition (`+`)
- Subtraction (`-`)
- Multiplication (`*`)
- Division (`/`)

### 2. Geometric Calculations
Plugin provides also functions to calculate the **area** of various geometric shapes.

### 3. Custom String-to-Integer Parser
A key challenge of this project is developing a mechanism to **convert numerical string values into integers**, **without relying on built-in parsing functions** such as `int.Parse`, `TryParse`, or `double.Parse`. The custom parser is designed to:
- **Interpret** numerical string inputs like `"123"` as integers
- **Handle edge cases**, such as negative numbers and invalid input formats
- **Ensure robustness** through error handling
