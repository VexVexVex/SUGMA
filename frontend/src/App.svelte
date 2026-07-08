<script>
  import { onMount } from 'svelte'
  import { getTodos, addTodo, toggleTodo, deleteTodo } from './lib/api.js'

  let todos = []
  let title = ''
  let dueAt = ''

  onMount(() => loadTodos())

  async function loadTodos() {
    todos = await getTodos()
  }

  async function handleAdd() {
    if (!title.trim()) return
    const due = dueAt ? new Date(dueAt).toISOString() : null
    await addTodo(title.trim(), due)
    title = ''
    dueAt = ''
    await loadTodos()
  }

  async function handleToggle(id) {
    await toggleTodo(id)
    await loadTodos()
  }

  async function handleDelete(id) {
    await deleteTodo(id)
    await loadTodos()
  }
</script>

<main>
  <h1>SUGMA Todos</h1>

  <form on:submit|preventDefault={handleAdd}>
    <input type="text" bind:value={title} placeholder="What needs to be done?" />
    <input type="datetime-local" bind:value={dueAt} />
    <button type="submit">Add</button>
  </form>

  <ul>
    {#each todos as todo (todo.id)}
      <li class:done={todo.isComplete}>
        <label>
          <input
            type="checkbox"
            checked={todo.isComplete}
            on:change={() => handleToggle(todo.id)}
          />
          <span>{todo.title}</span>
        </label>
        {#if todo.dueAt}
          <small>due {new Date(todo.dueAt).toLocaleDateString()}</small>
        {/if}
        <button on:click={() => handleDelete(todo.id)}>✕</button>
      </li>
    {/each}
  </ul>
</main>

<style>
  :global(*) {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }

  :global(body) {
    font-family: system-ui, sans-serif;
    background: #f5f5f5;
    display: flex;
    justify-content: center;
    padding: 2rem;
  }

  main {
    width: 100%;
    max-width: 500px;
    background: #fff;
    border-radius: 8px;
    padding: 2rem;
    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  }

  h1 {
    margin-bottom: 1rem;
    font-size: 1.5rem;
    color: #333;
  }

  form {
    display: flex;
    gap: 0.5rem;
    margin-bottom: 1rem;
  }

  input[type="text"] {
    flex: 1;
    padding: 0.5rem;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 1rem;
  }

  input[type="datetime-local"] {
    padding: 0.5rem;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 0.9rem;
  }

  button[type="submit"] {
    padding: 0.5rem 1rem;
    background: #4f46e5;
    color: #fff;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }

  ul {
    list-style: none;
  }

  li {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    padding: 0.5rem 0;
    border-bottom: 1px solid #eee;
  }

  li:last-child {
    border-bottom: none;
  }

  li.done span {
    text-decoration: line-through;
    color: #999;
  }

  li label {
    flex: 1;
    display: flex;
    align-items: center;
    gap: 0.5rem;
    cursor: pointer;
  }

  li small {
    color: #999;
    font-size: 0.8rem;
  }

  li button {
    background: none;
    border: none;
    color: #e11d48;
    cursor: pointer;
    font-size: 1rem;
  }
</style>
