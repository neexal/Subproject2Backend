import React from 'react';
import { Pagination } from 'react-bootstrap';

const CustomPagination = ({ page, totalPages, onPageChange }) => {
    // Logic to show a window of pages
    const getPageItems = () => {
        let items = [];
        let startPage = Math.max(1, page - 2);
        let endPage = Math.min(totalPages, page + 2);

        if (page <= 3) {
            endPage = Math.min(totalPages, 5);
        }
        if (page >= totalPages - 2) {
            startPage = Math.max(1, totalPages - 4);
        }

        for (let number = startPage; number <= endPage; number++) {
            items.push(
                <Pagination.Item key={number} active={number === page} onClick={() => onPageChange(number)}>
                    {number}
                </Pagination.Item>,
            );
        }
        return items;
    };

    return (
        <Pagination className="justify-content-center mt-4">
            <Pagination.First onClick={() => onPageChange(1)} disabled={page === 1} />
            <Pagination.Prev onClick={() => onPageChange(page - 1)} disabled={page === 1} />
            {getPageItems()}
            <Pagination.Next onClick={() => onPageChange(page + 1)} disabled={page === totalPages} />
            <Pagination.Last onClick={() => onPageChange(totalPages)} disabled={page === totalPages} />
        </Pagination>
    );
};

export default CustomPagination;
