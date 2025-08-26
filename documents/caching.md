# Section 2: Caching 

## Core architecture

- Front door: CDN for static assets + API Gateway / Load Balancer.
- Compute: stateless API services (containers or serverless) behind autoscaling.
- Data: primary relational DB (e.g. Postgres) with read replicas, Redis for ephemeral data and caching
- Other: message queues for emails, analytics ETL etc.

## Redis Strategy

Purpose: I'd use redis to speed up read-heavy endpoints/operations and maybe for some orchestration e.g. job scheduling.

Caching Examples:

- Event listing pages with short TTLs (< 5m).
- Precomputed aggregates (e.g. top 5 sales) updated asynchronously (could also use materialised views for this for this).
- Rate-limit counters or session tokens

Guidelines:

- Fallback to postgres etc if not found in redis
- Short TTLs.
- Update/expire cache after DB commit or rely on short TTLs.
- Use scheduled jobs/scripts to monitor and maintain cache health

## Initial Questions

- Acceptable staleness cached data (drives TTLs - affects performance)?
- Requirements for cache correctness?
- read/write ratio for each type of data?
