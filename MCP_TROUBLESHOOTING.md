# Supabase MCP Server Troubleshooting Guide

## Common Issues and Solutions

### 1. Configuration File Location

In Cursor, MCP servers are typically configured in one of these locations:
- **Settings UI**: Cursor Settings → Features → Model Context Protocol
- **Config File**: Usually in `%APPDATA%\Cursor\User\settings.json` or a dedicated MCP config file

### 2. Correct Supabase MCP Configuration

For Windows, use this configuration format:

```json
{
  "mcpServers": {
    "supabase": {
      "command": "cmd",
      "args": [
        "/c",
        "npx",
        "-y",
        "@modelcontextprotocol/server-postgres",
        "postgresql://postgres:[YOUR-PASSWORD]@db.[YOUR-PROJECT-REF].supabase.co:5432/postgres"
      ],
      "env": {
        "SUPABASE_PROJECT_REF": "your-project-ref",
        "SUPABASE_DB_PASSWORD": "your-db-password",
        "SUPABASE_REGION": "us-east-1"
      }
    }
  }
}
```

### 3. Connection String Format

Your Supabase connection string should be:
```
postgresql://postgres:[PASSWORD]@db.[PROJECT-REF].supabase.co:5432/postgres
```

Or using Supavisor (IPv4 compatible):
```
postgresql://postgres.[PROJECT-REF]:[PASSWORD]@aws-0-[REGION].pooler.supabase.com:6543/postgres
```

### 4. Common Errors

**Error: "Client closed" or connection timeout**
- Solution: Use `cmd /c` wrapper for Windows (as shown above)
- Alternative: Use Supavisor connection string (IPv4 compatible)

**Error: "Cannot find module"**
- Solution: Ensure Node.js is installed and in PATH
- Run: `npx -y @modelcontextprotocol/server-postgres --help` to test

**Error: Authentication failed**
- Check your database password is correct
- Verify project reference is correct
- Ensure IP allowlist includes your IP (if enabled)

### 5. Testing the Connection

Test the MCP server manually:
```powershell
npx -y @modelcontextprotocol/server-postgres "postgresql://postgres:[PASSWORD]@db.[PROJECT-REF].supabase.co:5432/postgres"
```

### 6. Environment Variables (Alternative Method)

Create a `.env` file at: `%APPDATA%\supabase-mcp\.env`

```
SUPABASE_PROJECT_REF=your-project-ref
SUPABASE_DB_PASSWORD=your-db-password
SUPABASE_REGION=us-east-1
```

Then use a simpler command:
```json
{
  "mcpServers": {
    "supabase": {
      "command": "cmd",
      "args": ["/c", "npx", "-y", "@modelcontextprotocol/server-postgres"]
    }
  }
}
```

## Next Steps

1. Check Cursor's MCP settings in the UI
2. Verify your Supabase connection string
3. Test the connection manually using npx
4. Review Cursor's output/logs for specific error messages

