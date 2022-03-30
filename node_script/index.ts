import ValApi from '@ing3kth/val-api'
import { isAfter, addSeconds } from 'date-fns'
import http from 'http'

const username = process.env.RIOT_USERNAME!
const password = process.env.RIOT_PASSWORD!

function getClient() {
  return
}

async function main() {
  console.log('Starting server...!')
  console.log(process.env)
  console.log({ username, password, env: process.env.NODE_ENV })

  let account = await ValApi.Auth.Account.login(username, password)
  let expiresAt = new Date()

  const server = http.createServer(async (req, res) => {
    console.log('Got request!')
    if (!account || isAfter(new Date(), expiresAt)) {
      console.log('Getting new client!')
      account = await ValApi.Auth.Account.login(username, password)
      expiresAt = addSeconds(new Date(), 3500)
    }

    const tokens = {
      accessToken: account.accessToken,
      entitlementToken: account.entitlements,
      expiresAt,
    }
    console.log('Got tokens: ', tokens)

    res.write(JSON.stringify(tokens))
    res.end()
  })

  server.listen(process.env.PORT ?? 5001)
}

main().catch(console.error)
