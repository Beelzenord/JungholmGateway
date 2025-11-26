# Supabase MCP Server Setup for Cursor

## Step 1: Access Cursor MCP Settings

In Cursor, MCP servers are configured through the Settings UI:
1. Open Cursor Settings (`Ctrl+,` or `File → Preferences → Settings`)
2. Search for "MCP" or "Model Context Protocol"
3. Look for "MCP Servers" or "Features → Model Context Protocol"

## Step 2: Configuration Format for Windows

Use this configuration in Cursor's MCP settings:

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
      ]
    }
  }
}
```

**Replace:**
- `[YOUR-PASSWORD]` with your Supabase database password
- `[YOUR-PROJECT-REF]` with your Supabase project reference (found in your Supabase dashboard URL)

## Step 3: Alternative - Using Environment Variables

If you prefer to keep credentials out of the config file:

1. Create `.env` file at: `%APPDATA%\supabase-mcp\.env`
   ```
   SUPABASE_PROJECT_REF=your-project-ref
   SUPABASE_DB_PASSWORD=your-db-password
   SUPABASE_REGION=us-east-1
   ```

2. Use this simpler config:
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
           "postgresql://postgres.${SUPABASE_PROJECT_REF}:${SUPABASE_DB_PASSWORD}@aws-0-${SUPABASE_REGION}.pooler.supabase.com:6543/postgres"
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

## Step 4: Test Connection Manually

Before configuring in Cursor, test the connection:

```powershell
# Replace with your actual credentials
$password = "your-password"
$projectRef = "your-project-ref"
npx -y @modelcontextprotocol/server-postgres "postgresql://postgres:$password@db.$projectRef.supabase.co:5432/postgres"
```

If this works, the MCP server is functioning correctly.

## Step 5: Common Issues

### Issue: "Client closed" or Connection Timeout
**Solution:** Use Supavisor connection string (IPv4 compatible):
```
postgresql://postgres.[PROJECT-REF]:[PASSWORD]@aws-0-[REGION].pooler.supabase.com:6543/postgres
```

### Issue: "Cannot find module"
**Solution:** 
- Ensure Node.js is in PATH: `node --version`
- Clear npx cache: `npx clear-npx-cache`

### Issue: "Authentication failed"
**Solution:**
- Verify password is correct (no extra spaces)
- Check project reference matches your Supabase dashboard
- Ensure IP allowlist includes your IP (if enabled in Supabase)

### Issue: IPv6 Connection Errors
**Solution:** Use Supavisor connection string (see Issue 1)

## Step 6: Verify Configuration

After adding the MCP server in Cursor:
1. Restart Cursor
2. Check Cursor's output/logs for MCP connection status
3. Try using MCP resources: `@list_mcp_resources` or check available tools

## Finding Your Supabase Credentials

1. **Project Reference**: Found in your Supabase dashboard URL
   - Example: `https://app.supabase.com/project/abcdefghijklmnop` → `abcdefghijklmnop`

2. **Database Password**: 
   - Go to Project Settings → Database
   - Or reset it if needed

3. **Region**: 
   - Found in Project Settings → General
   - Common: `us-east-1`, `us-west-1`, `eu-west-1`, etc.

