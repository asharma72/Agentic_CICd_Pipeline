import { useState, useEffect } from 'react';
import { getAll, create, update, remove } from './api/client';
import ItemList from './components/ItemList';
import ItemForm from './components/ItemForm';

export default function App() {
  const [items,   setItems]   = useState([]);
  const [editing, setEditing] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error,   setError]   = useState(null);

  const load = async () => {
    try {
      setLoading(true);
      const res = await getAll();
      setItems(res.data);
    } catch (e) {
      setError(e.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const handleCreate = async (data) => {
    await create(data);
    load();
  };

  const handleUpdate = async (id, data) => {
    await update(id, data);
    setEditing(null);
    load();
  };

  const handleDelete = async (id) => {
    await remove(id);
    load();
  };

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: '24px 16px' }}>
      <h1 style={{ fontSize: 28, fontWeight: 700, marginBottom: 24 }}>
        Ecommerce Management
      </h1>
      <ItemForm
        initial={{editing}}
        onSubmit={editing
          ? (data) => handleUpdate(editing.id, data)
          : handleCreate}
        onCancel={() => setEditing(null)}
      />
      {loading && <p>Loading...</p>}
      {error   && <p style={{color:'red'}}>{}</p>}
      {!loading && (
        <ItemList
          items={{items}}
          onEdit={setEditing}
          onDelete={handleDelete}
        />
      )}
    </div>
  );
}
