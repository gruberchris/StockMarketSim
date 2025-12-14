---
agent: 'agent'
description: 'Analyze a codebase and generate or update comprehensive .github/copilot-instructions.md files with project-specific conventions, patterns, and best practices.'
tools: ['search', 'search/codebase', 'edit/editFiles', 'changes', 'runCommands', 'runCommands/terminalLastCommand', 'problems', 'extensions', 'fetch', 'githubRepo', 'usages']
---

# Create or Update GitHub Copilot Instructions

This prompt helps analyze a codebase and generate or update comprehensive `.github/copilot-instructions.md` files that provide AI assistants with context about the project's conventions, patterns, and best practices.

## Objective

Analyze the codebase and create/update `.github/copilot-instructions.md` with comprehensive guidance that helps GitHub Copilot and other AI assistants provide better, more contextually appropriate suggestions.

## Instructions

### Step 1: Search for Existing AI Convention Files

Look for any existing AI configuration or instruction files in the repository:

1. Check for `.github/copilot-instructions.md`
2. Check for `.copilot-instructions.md` in the root directory
3. Look for `.github/copilot/instructions.md` or similar variants
4. Search for other AI-related configuration files (e.g., `.cursor/`, `.aider/`, etc.)
5. Note any existing content to preserve important information

**Commands to use:**
```bash
find . -type f -name "*copilot*" -o -name "*ai*" -o -name "*instructions*" | grep -E "\.(md|txt|yml|yaml)$"
ls -la .github/copilot-instructions.md 2>/dev/null || echo "Not found"
ls -la .copilot-instructions.md 2>/dev/null || echo "Not found"
```

### Step 2: Analyze the Codebase

Examine the repository structure and key files to understand the project:

#### 2.1 Package Configuration Files

Identify the technology stack and dependencies:

- **Node.js/JavaScript/TypeScript**: `package.json`, `package-lock.json`, `yarn.lock`, `pnpm-lock.yaml`
- **Python**: `requirements.txt`, `pyproject.toml`, `setup.py`, `Pipfile`, `poetry.lock`
- **Java/Kotlin**: `pom.xml`, `build.gradle`, `build.gradle.kts`, `settings.gradle`
- **C#/.NET**: `*.csproj`, `*.sln`, `*.slnx`, `packages.config`, `Directory.Build.props`
- **Ruby**: `Gemfile`, `Gemfile.lock`
- **Go**: `go.mod`, `go.sum`
- **Rust**: `Cargo.toml`, `Cargo.lock`
- **PHP**: `composer.json`, `composer.lock`

**Extract:**
- Main dependencies and frameworks used
- Development dependencies and tooling
- Target runtime versions
- Build systems and task runners

#### 2.2 Main Entry Points and Core Modules

Identify the application structure:

- Look for main entry points (e.g., `main.py`, `index.js`, `Program.cs`, `main.go`)
- Examine directory structure (`src/`, `lib/`, `app/`, `tests/`, etc.)
- Identify core modules and their responsibilities
- Note the architecture pattern (MVC, microservices, monolith, etc.)

**Common patterns to identify:**
```bash
# Find main entry points
find . -name "main.*" -o -name "index.*" -o -name "app.*" -o -name "server.*" | head -20

# Understand directory structure
tree -L 2 -d . || find . -type d -maxdepth 2
```

#### 2.3 Documentation Files

Review existing documentation:

- `README.md` - Project overview, setup instructions, usage
- `CONTRIBUTING.md` - Contribution guidelines and workflows
- `CHANGELOG.md` - Version history and changes
- `docs/` directory - Detailed documentation
- `CODE_OF_CONDUCT.md` - Community guidelines
- `LICENSE` - License information
- Any `*.md` files in the root or docs directories

**Extract:**
- Project purpose and goals
- Setup and installation procedures
- Development workflows
- Testing strategies
- Deployment processes
- Existing coding standards mentioned

#### 2.4 Project Structure and Architecture

Analyze the organization:

- Monorepo vs single project
- Frontend/backend separation
- Microservices architecture
- Module organization patterns
- Testing structure (unit, integration, e2e)
- Configuration management approach

**Look for:**
- `lerna.json`, `nx.json`, `workspace` configuration (monorepo indicators)
- `docker-compose.yml`, `Dockerfile` (containerization)
- CI/CD configuration (`.github/workflows/`, `.gitlab-ci.yml`, `azure-pipelines.yml`)
- Infrastructure as Code (`terraform/`, `kubernetes/`, `helm/`)

### Step 3: Identify Coding Conventions and Patterns

Examine source code to identify:

#### 3.1 Code Style and Formatting

- Indentation (spaces vs tabs, size)
- Naming conventions (camelCase, PascalCase, snake_case, kebab-case)
- File naming patterns
- Line length limits
- Quote style (single vs double)

**Configuration files to check:**
- `.editorconfig`
- `.prettierrc`, `prettier.config.js`
- `.eslintrc`, `eslint.config.js`
- `pyproject.toml` (black, ruff, mypy sections)
- `.rubocop.yml`
- `StyleCop.ruleset`, `.editorconfig` (C#)
- `rustfmt.toml`

#### 3.2 Language-Specific Patterns

**JavaScript/TypeScript:**
- Prefer `const`/`let` over `var`
- Arrow functions vs function declarations
- Async/await vs promises vs callbacks
- Module system (ESM vs CommonJS)
- TypeScript strict mode settings

**Python:**
- Type hints usage
- Docstring style (Google, NumPy, Sphinx)
- Import organization
- Class vs function preference
- Error handling patterns

**C#/.NET:**
- Nullable reference types usage
- LINQ patterns
- Async/await patterns
- Dependency injection approach
- Record types vs classes

**Java:**
- Annotation usage
- Stream API patterns
- Optional handling
- Exception handling approach

#### 3.3 Testing Approaches

Identify:
- Testing frameworks (Jest, pytest, xUnit, JUnit, etc.)
- Test organization (co-located vs separate directories)
- Naming conventions for test files/functions
- Mocking/stubbing libraries
- Code coverage tools and thresholds
- E2E testing approach

**Files to examine:**
```bash
# Find test files
find . -name "*test*" -o -name "*spec*" | head -20

# Find test configuration
find . -name "jest.config.*" -o -name "pytest.ini" -o -name "phpunit.xml"
```

#### 3.4 Error Handling and Logging

- Error handling patterns (try/catch, Result types, error boundaries)
- Logging libraries and patterns
- Error message conventions
- Debug logging approach

### Step 4: Generate Copilot Instructions

Create `.github/copilot-instructions.md` with the following sections:

```markdown
# GitHub Copilot Instructions for [Project Name]

## Project Overview

[Brief description of what this project does, its purpose, and its main features]

**Technology Stack:**
- [List main technologies, frameworks, and languages]

**Project Type:** [Web app, CLI tool, library, API, etc.]

## Architecture and Design Patterns

[Describe the high-level architecture]

**Key Architectural Decisions:**
- [List important architectural patterns used]
- [Note any specific design patterns commonly used]

**Directory Structure:**
```
[Provide overview of main directories and their purposes]
```

## Coding Conventions

### General Guidelines

- **Language:** [Primary language and version]
- **Style Guide:** [Reference to style guide if any, e.g., "Follow Airbnb JavaScript Style Guide"]
- **Formatting:** [Note about formatters used, e.g., "Code is formatted with Prettier"]
- **Linting:** [Linters used and configuration]

### Naming Conventions

- **Files:** [File naming pattern, e.g., kebab-case, PascalCase]
- **Variables:** [Variable naming pattern]
- **Functions/Methods:** [Function naming pattern]
- **Classes:** [Class naming pattern]
- **Constants:** [Constant naming pattern]
- **Test Files:** [Test file naming pattern]

### Code Organization

- [Where to place different types of code]
- [Module organization patterns]
- [Import/export conventions]

### Language-Specific Guidelines

[Language-specific best practices and patterns used in this codebase]

**Preferred Patterns:**
```[language]
// Example of preferred pattern
```

**Anti-Patterns to Avoid:**
```[language]
// Example of what NOT to do
```

## Best Practices

### Error Handling

- [How errors should be handled]
- [Error logging patterns]
- [Custom error types if used]

### Async Operations

- [How to handle async code]
- [Preferred async patterns]

### State Management

- [State management approach if applicable]
- [State mutation guidelines]

### Performance Considerations

- [Any performance-critical areas]
- [Optimization patterns]

### Security Guidelines

- [Input validation patterns]
- [Authentication/authorization approach]
- [Sensitive data handling]
- [Security best practices for this project]

## Testing

**Testing Framework:** [Framework name and version]

**Test Organization:**
- [Where tests are located]
- [Test file naming convention]

**Writing Tests:**
- [Test naming conventions]
- [What to test and what to skip]
- [Mocking/stubbing patterns]
- [Coverage expectations]

**Running Tests:**
```bash
[Command to run tests]
[Command to run specific test suites]
[Command to generate coverage]
```

## Common Patterns

### [Pattern Name 1]

**When to Use:** [Description]

**Example:**
```[language]
// Code example
```

### [Pattern Name 2]

**When to Use:** [Description]

**Example:**
```[language]
// Code example
```

## Common Anti-Patterns

### [Anti-Pattern Name 1]

**Why to Avoid:** [Reason]

**Instead, Do This:**
```[language]
// Better approach
```

## Dependencies

**Adding Dependencies:**
- [Process for adding new dependencies]
- [Approval process if any]
- [Security scanning requirements]

**Updating Dependencies:**
- [How and when to update]
- [Testing requirements before updating]

**Dependency Guidelines:**
- [Prefer certain libraries over others]
- [Deprecated libraries to avoid]

## Development Workflow

**Setup:**
```bash
[Commands to set up development environment]
```

**Building:**
```bash
[Commands to build the project]
```

**Running Locally:**
```bash
[Commands to run the project]
```

**Code Quality:**
```bash
[Linting commands]
[Formatting commands]
[Type checking commands]
```

## Documentation

- [Where to add documentation]
- [Documentation style and format]
- [Code comment conventions]
- [API documentation approach if applicable]

## CI/CD

- [CI/CD platform used]
- [Automated checks that run]
- [Deployment process]
- [Branch protection rules]

## Additional Context

[Any other important information that would help Copilot provide better suggestions]

## Resources

- [Links to important documentation]
- [Links to style guides]
- [Links to architectural decision records]
```

### Step 5: Consolidate and Synthesize

When creating the instructions:

1. **Merge existing information** from any found AI instruction files
2. **Extract actual patterns** from the code rather than assuming conventions
3. **Be specific** - use examples from the actual codebase
4. **Prioritize** the most important and frequently-used patterns
5. **Keep it concise** - focus on what's unique to this project
6. **Use code examples** from the actual codebase when possible
7. **Update, don't replace** - if instructions exist, enhance them with new findings

### Step 6: Validation

Before finalizing:

1. Ensure `.github/` directory exists (create if needed)
2. Validate markdown formatting
3. Check that all code examples use correct syntax highlighting
4. Ensure instructions are clear and actionable
5. Verify no sensitive information (API keys, passwords) is included

**Create directory if needed:**
```bash
mkdir -p .github
```

**Save the file:**
```bash
# Create or update the file at .github/copilot-instructions.md
```

## Tips for Effective Instructions

1. **Be Specific to This Codebase** - Don't just list general best practices; highlight what makes this codebase unique
2. **Include Examples** - Real code examples are more helpful than abstract descriptions
3. **Highlight Gotchas** - Mention common mistakes or tricky areas
4. **Keep It Updated** - These instructions should evolve with the codebase
5. **Focus on Patterns** - Help Copilot recognize and replicate successful patterns
6. **Document Decisions** - Explain why certain approaches are preferred
7. **Be Concise** - Copilot works better with focused, clear instructions
8. **Use Proper Formatting** - Well-formatted markdown improves AI comprehension

## Output

After completing the analysis and generation:

1. Report the location of the created/updated file
2. Summarize key conventions discovered
3. List any existing AI instruction files that were consolidated
4. Recommend any follow-up actions (e.g., updating outdated sections)

---

**Note:** This prompt is designed to be repository-agnostic. Adapt the level of detail based on the project size and complexity. For larger projects, consider creating additional focused instruction files for specific subsystems or modules.