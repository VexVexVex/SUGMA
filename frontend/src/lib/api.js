import { GraphQLClient } from 'graphql-request'

const endpoint = typeof window !== 'undefined'
  ? `${window.location.origin}/graphql`
  : '/graphql'

const client = new GraphQLClient(endpoint)

export async function getTodos() {
  const query = `
    query {
      todos {
        id
        title
        isComplete
        createdAt
        dueAt
      }
    }
  `
  const data = await client.request(query)
  return data.todos
}

export async function addTodo(title, dueAt) {
  const mutation = `
    mutation($title: String!, $dueAt: DateTime) {
      addTodo(title: $title, dueAt: $dueAt) {
        id
        title
        isComplete
        createdAt
        dueAt
      }
    }
  `
  const data = await client.request(mutation, { title, dueAt })
  return data.addTodo
}

export async function toggleTodo(id) {
  const mutation = `
    mutation($id: Int!) {
      toggleTodo(id: $id) {
        id
        isComplete
      }
    }
  `
  const data = await client.request(mutation, { id })
  return data.toggleTodo
}

export async function deleteTodo(id) {
  const mutation = `
    mutation($id: Int!) {
      deleteTodo(id: $id)
    }
  `
  const data = await client.request(mutation, { id })
  return data.deleteTodo
}
