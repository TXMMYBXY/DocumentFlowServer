# DocumentFlowServer

## Содержание

- [Содержание](#содержание)
- [Информация](#информация)
- [Стек](#стек)
- [Требования](#требования)
- [Возможности](#возможности)
- [Структура проекта](#структура-проекта)
- [Быстрый старт](#быстрый-старт)
- [Скрипт деплоя](#скрипт-деплоя)
- [Документация API](#документация-api)

## Информация

DocumentFlowServer — серверная часть на платформе ASP.NET Core (.NET 9) с использованием MySQL, Redis и фоновой службы (BackgroundWorker), является Backend-ом "ПО для формирования документов". Сервис распределяет запросы от клиентского приложения, работает с базой данных и передаёт задачи фоновой службе для формирования документов.
Через WebAPI происходит взаимодействие с сущностями пользователей, рефреш-токенов, отделов, логинов пользователей, шаблонов, документов(созданным из шаблонов) и задач, реализуемых очередность документов на формирование.

## Стек

- ASP .NET Core/.NET 9
  - EF Core V9.0.9
  - Microsoft.AspNetCore.Identity V2.3.9
  - DocumentFormat.OpenXml V2.13.1
  - Microsoft.AspNetCore.SignalR V1.2.9
  - AutoMapper V12.0.1
- Redis 7-alpine
- MySQL 8.0
- Worker/.NET 8
- JSON
- HTTP
- Docker
- JWT

## Требования

- Docker (и поддержка docker compose через `docker compose`)
- Git
- Bash (на Windows — WSL или Git Bash)

## Возможности

- Авторизация с помощью JWT токена и обновление его через механизм Refresh
- CRUD с сущностями пользователей, документов и шаблонов
- Скачивание и загрузка файлов на сервер, в бд хранятся метаданные, а в томе бинарные файлы
- Документы формируются фоновой службой
- Склонение по падежам по ключам

## Структура проекта
```
DocumentFlowServer
│
├── DocumentFlowServer.Api              → Web API (Controllers, Middleware)
├── DocumentFlowServer.Application      → Бизнес-логика, DTO, сервисы
├── DocumentFlowServer.Entities         → Модели и DbContext
├── DocumentFlowServer.Infrastructure   → Репозитории, реализация зависимостей
├── DocumentFlowServer.Worker           → Проект фоновой службы
├── compose.yml                         → Compose файл
├── deploy.sh                           → Скрипт запуска системы
├── READMY.md                           → Этот файл
└── .env.example                        → Файл переменных окружения
```

## Быстрый старт

1. Клонируйте репозиторий:

```bash
git clone https://github.com/TXMMYBXY/DocumentFlowServer.git
cd DocumentFlowServer
```

2. Создайте файл с переменными окружения на основе примера:

```bash
cp .env.example .env
# отредактируйте .env, например:
nano .env

# или
vim .env
```

3. Поднимите сервисы:

```bash
docker compose up -d --build
```

4. Проверьте статус контейнеров:

```bash
docker compose ps
```

## Скрипт деплоя

В корне репозитория находится скрипт `deploy.sh`, который:
- проверяет доступность Docker и Git,
- позволяет указать ветку для деплоя (по умолчанию текущая ветка),
- делает безопасный git fetch && git reset --hard на указанной ветке,
- перезапускает сервисы через docker compose и собирает образы заново,
- очищает неиспользуемые образы и временные ресурсы.

Примеры использования:

```bash
# дать разрешение на выполнение (один раз)
chmod +x deploy.sh

# запустить скрипт для текущей ветки
./deploy.sh

# запустить скрипт и явно указать ветку (например main)
./deploy.sh main
```

Если вы используете Windows без Bash, запустите скрипт внутри WSL или адаптируйте команды под PowerShell.

## Документация API
<details>
<summary>Документация</summary>

```json
{
  "openapi": "3.0.4",
  "info": {
    "title": "Document Flow Server",
    "version": "v2"
  },
  "paths": {
    "/api/authorization/login": {
      "post": {
        "tags": [
          "Authorization"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/LoginRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/LoginRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/LoginRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/LoginResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/LoginResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/LoginResponse"
                }
              }
            }
          }
        }
      }
    },
    "/api/authorization/request-for-access": {
      "post": {
        "tags": [
          "Authorization"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/RefreshTokenLoginRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/RefreshTokenLoginRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/RefreshTokenLoginRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/LoginRefreshResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/LoginRefreshResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/LoginRefreshResponse"
                }
              }
            }
          }
        }
      }
    },
    "/api/authorization/access": {
      "post": {
        "tags": [
          "Authorization"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/AccessTokenRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/AccessTokenRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/AccessTokenRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/AccessTokenResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/AccessTokenResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/AccessTokenResponse"
                }
              }
            }
          }
        }
      }
    },
    "/api/department": {
      "get": {
        "tags": [
          "Department"
        ],
        "parameters": [
          {
            "name": "SortBy",
            "in": "query",
            "schema": {
              "$ref": "#/components/schemas/DepartmentSortField"
            }
          },
          {
            "name": "Descending",
            "in": "query",
            "schema": {
              "type": "boolean"
            }
          },
          {
            "name": "Title",
            "in": "query",
            "schema": {
              "type": "string"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/PagedDepartmentResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedDepartmentResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedDepartmentResponse"
                }
              }
            }
          }
        }
      },
      "post": {
        "tags": [
          "Department"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateDepartmentRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateDepartmentRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/CreateDepartmentRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/department/{departmentId}": {
      "patch": {
        "tags": [
          "Department"
        ],
        "parameters": [
          {
            "name": "departmentId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateDepartmentRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateDepartmentRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateDepartmentRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "delete": {
        "tags": [
          "Department"
        ],
        "parameters": [
          {
            "name": "departmentId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/document/{documentId}/download": {
      "get": {
        "tags": [
          "Document"
        ],
        "parameters": [
          {
            "name": "documentId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/document": {
      "get": {
        "tags": [
          "Document"
        ],
        "parameters": [
          {
            "name": "SortBy",
            "in": "query",
            "schema": {
              "$ref": "#/components/schemas/DocumentSortField"
            }
          },
          {
            "name": "Descending",
            "in": "query",
            "schema": {
              "type": "boolean"
            }
          },
          {
            "name": "Title",
            "in": "query",
            "schema": {
              "type": "string"
            }
          },
          {
            "name": "TemplateId",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "CreatedAtEarlier",
            "in": "query",
            "schema": {
              "type": "string",
              "format": "date-time"
            }
          },
          {
            "name": "CreatedAtLater",
            "in": "query",
            "schema": {
              "type": "string",
              "format": "date-time"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/PagedDocumentResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedDocumentResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedDocumentResponse"
                }
              }
            }
          }
        }
      }
    },
    "/api/document/{documentId}": {
      "delete": {
        "tags": [
          "Document"
        ],
        "parameters": [
          {
            "name": "documentId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/document/upload": {
      "post": {
        "tags": [
          "Document"
        ],
        "requestBody": {
          "content": {
            "multipart/form-data": {
              "schema": {
                "required": [
                  "CreatedBy",
                  "File",
                  "TemplateId",
                  "Title"
                ],
                "type": "object",
                "properties": {
                  "Title": {
                    "type": "string"
                  },
                  "CreatedBy": {
                    "type": "integer",
                    "format": "int32"
                  },
                  "TemplateId": {
                    "type": "integer",
                    "format": "int32"
                  },
                  "File": {
                    "type": "string",
                    "format": "binary"
                  }
                }
              },
              "encoding": {
                "Title": {
                  "style": "form"
                },
                "CreatedBy": {
                  "style": "form"
                },
                "TemplateId": {
                  "style": "form"
                },
                "File": {
                  "style": "form"
                }
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/issue/generate": {
      "post": {
        "tags": [
          "Issue"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateIssueRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateIssueRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/CreateIssueRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/personal": {
      "get": {
        "tags": [
          "PersonalAccount"
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/PersonResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PersonResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/PersonResponse"
                }
              }
            }
          }
        }
      }
    },
    "/api/personal/change-password": {
      "patch": {
        "tags": [
          "PersonalAccount"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/ChangePasswordRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/ChangePasswordRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/ChangePasswordRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/personal/login-times": {
      "get": {
        "tags": [
          "PersonalAccount"
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/LoginTimeResponse"
                  }
                }
              },
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/LoginTimeResponse"
                  }
                }
              },
              "text/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/LoginTimeResponse"
                  }
                }
              }
            }
          }
        }
      }
    },
    "/api/template": {
      "get": {
        "tags": [
          "Template"
        ],
        "parameters": [
          {
            "name": "SortBy",
            "in": "query",
            "schema": {
              "$ref": "#/components/schemas/TemplateSortField"
            }
          },
          {
            "name": "Descending",
            "in": "query",
            "schema": {
              "type": "boolean"
            }
          },
          {
            "name": "Type",
            "in": "query",
            "required": true,
            "schema": {
              "$ref": "#/components/schemas/TemplateType"
            }
          },
          {
            "name": "Title",
            "in": "query",
            "schema": {
              "type": "string"
            }
          },
          {
            "name": "CreatedBy",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "CreatedAtEarlier",
            "in": "query",
            "schema": {
              "type": "string",
              "format": "date-time"
            }
          },
          {
            "name": "CreatedAtLater",
            "in": "query",
            "schema": {
              "type": "string",
              "format": "date-time"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/PagedTemplateResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedTemplateResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedTemplateResponse"
                }
              }
            }
          }
        }
      },
      "post": {
        "tags": [
          "Template"
        ],
        "requestBody": {
          "content": {
            "multipart/form-data": {
              "schema": {
                "required": [
                  "File",
                  "Title",
                  "Type"
                ],
                "type": "object",
                "properties": {
                  "Title": {
                    "type": "string"
                  },
                  "IsActive": {
                    "type": "boolean"
                  },
                  "Type": {
                    "$ref": "#/components/schemas/TemplateType"
                  },
                  "File": {
                    "type": "string",
                    "format": "binary"
                  }
                }
              },
              "encoding": {
                "Title": {
                  "style": "form"
                },
                "IsActive": {
                  "style": "form"
                },
                "Type": {
                  "style": "form"
                },
                "File": {
                  "style": "form"
                }
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "delete": {
        "tags": [
          "Template"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/DeleteManyTemplatesRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/DeleteManyTemplatesRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/DeleteManyTemplatesRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/template/{templateId}/extract-fields": {
      "get": {
        "tags": [
          "Template"
        ],
        "parameters": [
          {
            "name": "templateId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/TemplateFieldInfoResponse"
                  }
                }
              },
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/TemplateFieldInfoResponse"
                  }
                }
              },
              "text/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/TemplateFieldInfoResponse"
                  }
                }
              }
            }
          }
        }
      }
    },
    "/api/template/{templateId}/download": {
      "get": {
        "tags": [
          "Template"
        ],
        "parameters": [
          {
            "name": "templateId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/template/{templateId}/change-template-status": {
      "patch": {
        "tags": [
          "Template"
        ],
        "parameters": [
          {
            "name": "templateId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "type": "boolean"
                }
              },
              "application/json": {
                "schema": {
                  "type": "boolean"
                }
              },
              "text/json": {
                "schema": {
                  "type": "boolean"
                }
              }
            }
          }
        }
      }
    },
    "/api/template/{templateId}/update-template": {
      "patch": {
        "tags": [
          "Template"
        ],
        "parameters": [
          {
            "name": "templateId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "multipart/form-data": {
              "schema": {
                "type": "object",
                "properties": {
                  "Title": {
                    "type": "string"
                  },
                  "File": {
                    "type": "string",
                    "format": "binary"
                  }
                }
              },
              "encoding": {
                "Title": {
                  "style": "form"
                },
                "File": {
                  "style": "form"
                }
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/template/{templateId}": {
      "delete": {
        "tags": [
          "Template"
        ],
        "parameters": [
          {
            "name": "templateId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/template/unique": {
      "get": {
        "tags": [
          "Template"
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/TemplateResponse"
                  }
                }
              },
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/TemplateResponse"
                  }
                }
              },
              "text/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/TemplateResponse"
                  }
                }
              }
            }
          }
        }
      }
    },
    "/api/user": {
      "get": {
        "tags": [
          "User"
        ],
        "parameters": [
          {
            "name": "SortBy",
            "in": "query",
            "schema": {
              "$ref": "#/components/schemas/UserSortField"
            }
          },
          {
            "name": "Descending",
            "in": "query",
            "schema": {
              "type": "boolean"
            }
          },
          {
            "name": "FullName",
            "in": "query",
            "schema": {
              "type": "string"
            }
          },
          {
            "name": "Email",
            "in": "query",
            "schema": {
              "type": "string"
            }
          },
          {
            "name": "DepartmentId",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "RoleId",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/PagedUserResponse"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedUserResponse"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/PagedUserResponse"
                }
              }
            }
          }
        }
      },
      "post": {
        "tags": [
          "User"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateUserRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateUserRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/CreateUserRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "delete": {
        "tags": [
          "User"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/DeleteManyUsersRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/DeleteManyUsersRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/DeleteManyUsersRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/user/{userId}/user-info": {
      "patch": {
        "tags": [
          "User"
        ],
        "parameters": [
          {
            "name": "userId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateUserRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateUserRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateUserRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/user/{userId}/reset-password": {
      "patch": {
        "tags": [
          "User"
        ],
        "parameters": [
          {
            "name": "userId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/SetUserPasswordRequest"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/SetUserPasswordRequest"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/SetUserPasswordRequest"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/user/{userId}/change-status": {
      "patch": {
        "tags": [
          "User"
        ],
        "parameters": [
          {
            "name": "userId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "type": "boolean"
                }
              },
              "application/json": {
                "schema": {
                  "type": "boolean"
                }
              },
              "text/json": {
                "schema": {
                  "type": "boolean"
                }
              }
            }
          }
        }
      }
    },
    "/api/user/{userId}": {
      "delete": {
        "tags": [
          "User"
        ],
        "parameters": [
          {
            "name": "userId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/internal/tasks/worker/next": {
      "post": {
        "tags": [
          "Worker"
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "text/plain": {
                "schema": {
                  "$ref": "#/components/schemas/WorkerTaskDto"
                }
              },
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/WorkerTaskDto"
                }
              },
              "text/json": {
                "schema": {
                  "$ref": "#/components/schemas/WorkerTaskDto"
                }
              }
            }
          }
        }
      }
    },
    "/api/internal/tasks/worker/{taskId}/complete": {
      "post": {
        "tags": [
          "Worker"
        ],
        "parameters": [
          {
            "name": "taskId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskCompletedDto"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskCompletedDto"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskCompletedDto"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/internal/tasks/worker/{taskId}/fail": {
      "post": {
        "tags": [
          "Worker"
        ],
        "parameters": [
          {
            "name": "taskId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskFailedDto"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskFailedDto"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskFailedDto"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/internal/tasks/worker/{taskId}/progress": {
      "post": {
        "tags": [
          "Worker"
        ],
        "parameters": [
          {
            "name": "taskId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskProgressDto"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskProgressDto"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/WorkerTaskProgressDto"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/internal/tasks/worker/{templateId}/template": {
      "get": {
        "tags": [
          "Worker"
        ],
        "parameters": [
          {
            "name": "templateId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "AccessTokenDto": {
        "type": "object",
        "properties": {
          "accessToken": {
            "type": "string",
            "nullable": true
          },
          "expiresAt": {
            "type": "string",
            "format": "date-time"
          },
          "tokenType": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "AccessTokenRequest": {
        "required": [
          "refreshToken"
        ],
        "type": "object",
        "properties": {
          "refreshToken": {
            "minLength": 1,
            "type": "string"
          }
        },
        "additionalProperties": false
      },
      "AccessTokenResponse": {
        "type": "object",
        "properties": {
          "accessToken": {
            "type": "string",
            "nullable": true
          },
          "expiresAt": {
            "type": "string",
            "format": "date-time"
          },
          "tokenType": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "ChangePasswordRequest": {
        "required": [
          "confirmPassword",
          "currentPassword",
          "newPassword"
        ],
        "type": "object",
        "properties": {
          "currentPassword": {
            "minLength": 1,
            "type": "string"
          },
          "newPassword": {
            "minLength": 1,
            "type": "string"
          },
          "confirmPassword": {
            "minLength": 1,
            "type": "string"
          }
        },
        "additionalProperties": false
      },
      "CreateDepartmentRequest": {
        "required": [
          "title"
        ],
        "type": "object",
        "properties": {
          "title": {
            "minLength": 1,
            "type": "string"
          },
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "CreateIssueRequest": {
        "type": "object",
        "properties": {
          "templateId": {
            "type": "integer",
            "format": "int32"
          },
          "templateType": {
            "$ref": "#/components/schemas/TemplateType"
          },
          "data": {
            "type": "object",
            "additionalProperties": {},
            "nullable": true
          },
          "priority": {
            "$ref": "#/components/schemas/IssuePriority"
          }
        },
        "additionalProperties": false
      },
      "CreateUserRequest": {
        "required": [
          "departmentId",
          "email",
          "fullName",
          "password",
          "role"
        ],
        "type": "object",
        "properties": {
          "fullName": {
            "minLength": 1,
            "type": "string"
          },
          "email": {
            "minLength": 1,
            "type": "string"
          },
          "password": {
            "minLength": 1,
            "type": "string"
          },
          "departmentId": {
            "type": "integer",
            "format": "int32"
          },
          "role": {
            "$ref": "#/components/schemas/Role"
          }
        },
        "additionalProperties": false
      },
      "DeleteManyTemplatesRequest": {
        "required": [
          "templateIds"
        ],
        "type": "object",
        "properties": {
          "templateIds": {
            "type": "array",
            "items": {
              "type": "integer",
              "format": "int32"
            }
          }
        },
        "additionalProperties": false
      },
      "DeleteManyUsersRequest": {
        "required": [
          "userIds"
        ],
        "type": "object",
        "properties": {
          "userIds": {
            "type": "array",
            "items": {
              "type": "integer",
              "format": "int32"
            }
          }
        },
        "additionalProperties": false
      },
      "DepartmentCleanDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "description": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "DepartmentDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "employees": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/UserCleanDto"
            },
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "DepartmentSortField": {
        "enum": [
          0,
          1
        ],
        "type": "integer",
        "format": "int32"
      },
      "DocumentDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "filePath": {
            "type": "string",
            "nullable": true
          },
          "user": {
            "$ref": "#/components/schemas/UserCleanDto"
          },
          "createdAt": {
            "type": "string",
            "format": "date-time"
          },
          "type": {
            "$ref": "#/components/schemas/TemplateType"
          },
          "template": {
            "$ref": "#/components/schemas/TemplateClearDto"
          }
        },
        "additionalProperties": false
      },
      "DocumentSortField": {
        "enum": [
          0,
          1,
          2,
          3
        ],
        "type": "integer",
        "format": "int32"
      },
      "IssuePriority": {
        "enum": [
          0,
          1,
          2
        ],
        "type": "integer",
        "format": "int32"
      },
      "LoginRefreshResponse": {
        "type": "object",
        "properties": {
          "isAllowed": {
            "type": "boolean"
          },
          "accessToken": {
            "$ref": "#/components/schemas/AccessTokenDto"
          },
          "refreshToken": {
            "$ref": "#/components/schemas/RefreshTokenDto"
          }
        },
        "additionalProperties": false
      },
      "LoginRequest": {
        "required": [
          "email",
          "password"
        ],
        "type": "object",
        "properties": {
          "email": {
            "minLength": 1,
            "type": "string"
          },
          "password": {
            "minLength": 1,
            "type": "string"
          }
        },
        "additionalProperties": false
      },
      "LoginResponse": {
        "type": "object",
        "properties": {
          "access": {
            "$ref": "#/components/schemas/AccessTokenDto"
          },
          "refresh": {
            "$ref": "#/components/schemas/RefreshTokenDto"
          },
          "userInfo": {
            "$ref": "#/components/schemas/UserInfoForLoginDto"
          }
        },
        "additionalProperties": false
      },
      "LoginTimeResponse": {
        "type": "object",
        "properties": {
          "loginTime": {
            "type": "string",
            "format": "date-time",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "PagedDepartmentResponse": {
        "type": "object",
        "properties": {
          "departments": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/DepartmentDto"
            },
            "nullable": true
          },
          "totalCount": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "currentPage": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "format": "int32",
            "readOnly": true
          }
        },
        "additionalProperties": false
      },
      "PagedDocumentResponse": {
        "type": "object",
        "properties": {
          "documents": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/DocumentDto"
            },
            "nullable": true
          },
          "totalCount": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "currentPage": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "format": "int32",
            "readOnly": true
          }
        },
        "additionalProperties": false
      },
      "PagedTemplateResponse": {
        "type": "object",
        "properties": {
          "totalCount": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "currentPage": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "format": "int32",
            "readOnly": true
          },
          "templates": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/TemplateDto"
            },
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "PagedUserResponse": {
        "type": "object",
        "properties": {
          "users": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/UserDto"
            },
            "nullable": true
          },
          "totalCount": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "currentPage": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "format": "int32",
            "readOnly": true
          }
        },
        "additionalProperties": false
      },
      "PersonResponse": {
        "type": "object",
        "properties": {
          "fullName": {
            "type": "string",
            "nullable": true
          },
          "email": {
            "type": "string",
            "nullable": true
          },
          "department": {
            "type": "string",
            "nullable": true
          },
          "role": {
            "$ref": "#/components/schemas/Role"
          }
        },
        "additionalProperties": false
      },
      "RefreshTokenDto": {
        "type": "object",
        "properties": {
          "refreshToken": {
            "type": "string",
            "nullable": true
          },
          "expiresAt": {
            "type": "string",
            "format": "date-time",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "RefreshTokenLoginRequest": {
        "required": [
          "refreshToken"
        ],
        "type": "object",
        "properties": {
          "refreshToken": {
            "minLength": 1,
            "type": "string"
          }
        },
        "additionalProperties": false
      },
      "Role": {
        "enum": [
          1,
          2,
          3,
          4
        ],
        "type": "integer",
        "format": "int32"
      },
      "SetUserPasswordRequest": {
        "required": [
          "password"
        ],
        "type": "object",
        "properties": {
          "password": {
            "minLength": 1,
            "type": "string"
          }
        },
        "additionalProperties": false
      },
      "TemplateClearDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "TemplateDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "path": {
            "type": "string",
            "nullable": true
          },
          "createdBy": {
            "$ref": "#/components/schemas/TemplateOwnerDto"
          },
          "createdAt": {
            "type": "string",
            "format": "date-time"
          },
          "isActive": {
            "type": "boolean"
          },
          "type": {
            "$ref": "#/components/schemas/TemplateType"
          }
        },
        "additionalProperties": false
      },
      "TemplateFieldInfoResponse": {
        "type": "object",
        "properties": {
          "key": {
            "type": "string",
            "nullable": true
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "type": {
            "type": "string",
            "nullable": true
          },
          "required": {
            "type": "boolean"
          },
          "options": {
            "type": "array",
            "items": {
              "type": "string"
            },
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "TemplateOwnerDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "fullName": {
            "type": "string",
            "nullable": true
          },
          "email": {
            "type": "string",
            "nullable": true
          },
          "role": {
            "$ref": "#/components/schemas/Role"
          }
        },
        "additionalProperties": false
      },
      "TemplateResponse": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "title": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "TemplateSortField": {
        "enum": [
          0,
          1,
          2,
          3
        ],
        "type": "integer",
        "format": "int32"
      },
      "TemplateType": {
        "enum": [
          0,
          1
        ],
        "type": "integer",
        "format": "int32"
      },
      "UpdateDepartmentRequest": {
        "type": "object",
        "properties": {
          "title": {
            "type": "string",
            "nullable": true
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "employeesIds": {
            "type": "array",
            "items": {
              "type": "integer",
              "format": "int32"
            },
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "UpdateUserRequest": {
        "type": "object",
        "properties": {
          "fullName": {
            "type": "string",
            "nullable": true
          },
          "email": {
            "type": "string",
            "nullable": true
          },
          "departmentId": {
            "type": "integer",
            "format": "int32",
            "nullable": true
          },
          "role": {
            "$ref": "#/components/schemas/Role"
          }
        },
        "additionalProperties": false
      },
      "UserCleanDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "fullName": {
            "type": "string",
            "nullable": true
          },
          "email": {
            "type": "string",
            "nullable": true
          },
          "isActive": {
            "type": "boolean"
          },
          "role": {
            "$ref": "#/components/schemas/Role"
          }
        },
        "additionalProperties": false
      },
      "UserDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "fullName": {
            "type": "string",
            "nullable": true
          },
          "email": {
            "type": "string",
            "nullable": true
          },
          "isActive": {
            "type": "boolean"
          },
          "department": {
            "$ref": "#/components/schemas/DepartmentCleanDto"
          },
          "role": {
            "$ref": "#/components/schemas/Role"
          }
        },
        "additionalProperties": false
      },
      "UserInfoForLoginDto": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "email": {
            "type": "string",
            "nullable": true
          },
          "department": {
            "type": "string",
            "nullable": true
          },
          "role": {
            "$ref": "#/components/schemas/Role"
          }
        },
        "additionalProperties": false
      },
      "UserSortField": {
        "enum": [
          0,
          1,
          2,
          3,
          4
        ],
        "type": "integer",
        "format": "int32"
      },
      "WorkerTaskCompletedDto": {
        "type": "object",
        "properties": {
          "resultFilePath": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "WorkerTaskDto": {
        "type": "object",
        "properties": {
          "taskId": {
            "type": "string",
            "format": "uuid"
          },
          "userId": {
            "type": "integer",
            "format": "int32"
          },
          "templateId": {
            "type": "integer",
            "format": "int32"
          },
          "templateType": {
            "$ref": "#/components/schemas/TemplateType"
          },
          "data": {
            "type": "object",
            "additionalProperties": {},
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "WorkerTaskFailedDto": {
        "type": "object",
        "properties": {
          "errorMessage": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "WorkerTaskProgressDto": {
        "type": "object",
        "properties": {
          "progress": {
            "type": "integer",
            "format": "int32"
          },
          "message": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      }
    },
    "securitySchemes": {
      "Bearer": {
        "type": "apiKey",
        "description": "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        "name": "Authorization",
        "in": "header"
      }
    }
  },
  "security": [
    {
      "Bearer": []
    }
  ]
}
```

</details>