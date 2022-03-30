FROM node:17 as base
WORKDIR /app

FROM base as build
WORKDIR /src
COPY . .

RUN npm install
RUN npm run build

FROM base as final
WORKDIR /app

COPY --from=build /src .
ENV NODE_ENV=production

ENTRYPOINT ["node", "build"]

