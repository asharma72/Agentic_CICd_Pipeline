import { useState, useEffect } from 'react';

export default function ItemForm({ initial, onSubmit, onCancel }) {
  const [form, setForm] = useState({
    name: '', description: '', price: 0, stock: 0
  });

  useEffect(() => {
    if (initial) setForm(initial);
    else setForm({ name: '', description: '', price: 0, stock: 0 });
  }, [initial]);

  const handle = e => {
    const val = e.target.type === 'number' ? parseFloat(e.target.value) : e.target.value;
    setForm(f => ({ ...f, [e.target.name]: val }));
  };

  const inputStyle = {
    padding: '8px 10px',
    border: '1px solid #ddd',
    borderRadius: 4,
    width: '100%',
    fontSize: 14
  };

  return (
    <div style={{ background: '#f9f9f9', padding: 16, borderRadius: 8, marginBottom: 24 }}>
      <h2 style={{ fontSize: 18, marginBottom: 12 }}>
        {initial ? 'Edit' : 'Add'} Ecommerce Item
      </h2>
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12, marginBottom: 12 }}>
        <div>
          <label style={{ fontSize: 12, color: '#666' }}>Name</label>
          <input name="name" value={form.name}
            onChange={handle} placeholder="Item name" style={inputStyle} />
        </div>
        <div>
          <label style={{ fontSize: 12, color: '#666' }}>Price</label>
          <input name="price" value={form.price}
            onChange={handle} placeholder="Price" type="number"
            min="0" step="0.01" style={inputStyle} />
        </div>
        <div>
          <label style={{ fontSize: 12, color: '#666' }}>Stock</label>
          <input name="stock" value={form.stock}
            onChange={handle} placeholder="Stock" type="number"
            min="0" style={inputStyle} />
        </div>
        <div>
          <label style={{ fontSize: 12, color: '#666' }}>Description</label>
          <input name="description" value={form.description}
            onChange={handle} placeholder="Description" style={inputStyle} />
        </div>
      </div>
      <div style={{ display: 'flex', gap: 8 }}>
        <button
          onClick={() => onSubmit(form)}
          style={{
            padding: '8px 20px',
            background: '#0070f3',
            color: '#fff',
            border: 'none',
            borderRadius: 4,
            cursor: 'pointer',
            fontSize: 14
          }}>
          {initial ? 'Update' : 'Create'}
        </button>
        {initial && (
          <button
            onClick={onCancel}
            style={{
              padding: '8px 20px',
              border: '1px solid #ddd',
              borderRadius: 4,
              cursor: 'pointer',
              fontSize: 14
            }}>
            Cancel
          </button>
        )}
      </div>
    </div>
  );
}
