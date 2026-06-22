#!/usr/bin/env bash

set -Eeuo pipefail

########################################
# Config
########################################

PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

########################################
# Helpers
########################################

log() {
    echo "[$(date '+%Y-%m-%d %H:%M:%S')] $*"
}

error() {
    echo "[ERROR] $*" >&2
    exit 1
}

check_command() {
    command -v "$1" >/dev/null 2>&1 || error "$1 is not installed"
}

########################################
# Dependency checks
########################################

log "Checking dependencies..."

check_command git
check_command docker

if docker compose version >/dev/null 2>&1; then
    COMPOSE="docker compose"
elif command -v docker-compose >/dev/null 2>&1; then
    COMPOSE="docker-compose"
else
    error "Docker Compose not found"
fi

########################################
# Repository
########################################

cd "$PROJECT_DIR"

CURRENT_BRANCH="$(git rev-parse --abbrev-ref HEAD)"
BRANCH="${1:-$CURRENT_BRANCH}"

log "Project directory: $PROJECT_DIR"
log "Deploy branch: $BRANCH"

########################################
# Git update
########################################

log "Fetching updates from origin..."

git fetch --prune origin

git show-ref --verify --quiet "refs/remotes/origin/$BRANCH" \
    || error "Branch origin/$BRANCH does not exist"

if [[ "$(git rev-parse --abbrev-ref HEAD)" != "$BRANCH" ]]; then
    log "Switching to branch $BRANCH..."
    git checkout "$BRANCH"
fi

log "Resetting to origin/$BRANCH..."
git reset --hard "origin/$BRANCH"

########################################
# Docker deploy
########################################

log "Pulling latest base images..."
$COMPOSE pull || true

log "Building and starting containers..."
$COMPOSE up -d --build --remove-orphans

########################################
# Cleanup
########################################

log "Cleaning Docker cache..."

docker image prune -f
docker builder prune -f

########################################
# Status
########################################

log "Containers status:"
$COMPOSE ps

log "Deployment completed successfully."