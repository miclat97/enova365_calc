> [!NOTE]
> Wyniki testów jednostkowych dla każdej z metod i każdego przypadku

![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/24testy_enova365.png)

> [!NOTE]
> Tak wygląda działanie pluginu w praktyce w systemie ERP Enova 365

![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/enova_1.png)
![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/enova_2.png)
![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/enova_3.png)
![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/enova_4.png)
![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/enova_5.png)
![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/enova_6.png)
![screenshot](https://raw.githubusercontent.com/miclat97/enova365_calc/refs/heads/main/Screenshots_Enova365/enova_7.png)


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
