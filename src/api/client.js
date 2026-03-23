import axios from 'axios';

const client = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'http://localhost:80',
  headers: { 'Content-Type': 'application/json' }
});

export const getAll  = ()          => client.get('/api/ecommerce');
export const getById = (id)        => client.get(`/api/ecommerce/${id}`);
export const create  = (data)      => client.post('/api/ecommerce', data);
export const update  = (id, data)  => client.put(`/api/ecommerce/${id}`, data);
export const remove  = (id)        => client.delete(`/api/ecommerce/${id}`);
