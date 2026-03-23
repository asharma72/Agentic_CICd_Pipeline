import React from 'react';

export default function ItemList({ items, onEdit, onDelete }) {
  if (!items.length) return <p style={{ color: '#888' }}>No items yet.</p>;
  return (
    <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: 16 }}>
      <thead>
        <tr style={{ background: '#f5f5f5' }}>
          <th style={{ padding: '10px 12px', textAlign: 'left' }}>ID</th>
          <th style={{ padding: '10px 12px', textAlign: 'left' }}>Name</th>
          <th style={{ padding: '10px 12px', textAlign: 'left' }}>Price</th>
          <th style={{ padding: '10px 12px', textAlign: 'left' }}>Stock</th>
          <th style={{ padding: '10px 12px', textAlign: 'left' }}>Actions</th>
        </tr>
      </thead>
      <tbody>
        {items.map(item => (
          <tr key={item.id} style={{ borderBottom: '1px solid #eee' }}>
            <td style={{ padding: '10px 12px' }}>#{item.id}</td>
            <td style={{ padding: '10px 12px' }}>{item.name}</td>
            <td style={{ padding: '10px 12px' }}>${item.price?.toFixed(2)}</td>
            <td style={{ padding: '10px 12px' }}>{item.stock}</td>
            <td style={{ padding: '10px 12px' }}>
              <button
                onClick={() => onEdit(item)}
                style={{ marginRight: 8, padding: '4px 10px', cursor: 'pointer' }}>
                Edit
              </button>
              <button
                onClick={() => onDelete(item.id)}
                style={{ padding: '4px 10px', cursor: 'pointer', color: 'red' }}>
                Delete
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
