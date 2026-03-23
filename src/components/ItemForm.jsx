import { useState, useEffect } from 'react';

export default function ItemForm({ initial, onSubmit, onCancel }) {
  const [form, setForm] = useState({
    name:'', description:'', price:0, stock:0
  });

  useEffect(() => {
    if (initial) setForm(initial);
  }, [initial]);

  const handle = e => setForm(f => ({ ...f, [e.target.name]: e.target.value }));

  return (
    <div style={{ background:'#f9f9f9', padding:16, borderRadius:8, marginBottom:24 }}>
      <h2 style={{ fontSize:18, marginBottom:12 }}>
        {initial ? 'Edit' : 'Add'} Ecommerce Item
      </h2>
      <div style={{ display:'grid', gridTemplateColumns:'1fr 1fr', gap:12 }}>
        <input name="name"        value={{form.name}}
          onChange={handle} placeholder="Name"
          style={{padding:'8px 10px', border:'1px solid #ddd', borderRadius:4}}/>
        <input name="price"       value={{form.price}}
          onChange={handle} placeholder="Price" type="number"
          style={{padding:'8px 10px', border:'1px solid #ddd', borderRadius:4}}/>
        <input name="stock"       value={{form.stock}}
          onChange={handle} placeholder="Stock" type="number"
          style={{padding:'8px 10px', border:'1px solid #ddd', borderRadius:4}}/>
        <input name="description" value={{form.description}}
          onChange={handle} placeholder="Description"
          style={{padding:'8px 10px', border:'1px solid #ddd', borderRadius:4}}/>
      </div>
      <div style={{ marginTop:12, display:'flex', gap:8 }}>
        <button onClick={() => onSubmit(form)}
          style={{padding:'8px 20px', background:'#0070f3', color:'#fff',
            border:'none', borderRadius:4, cursor:'pointer'}}>
          {initial ? 'Update' : 'Create'}
        </button>
        {initial && (
          <button onClick={onCancel}
            style={{padding:'8px 20px', border:'1px solid #ddd',
              borderRadius:4, cursor:'pointer'}}>
            Cancel
          </button>
        )}
      </div>
    </div>
  );
}
